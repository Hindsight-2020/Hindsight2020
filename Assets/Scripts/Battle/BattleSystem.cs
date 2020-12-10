using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { Start, ActionSelection, MoveSelection, PerformMove, Busy, PartyScreen, BattleOver}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] PartyScreen partyScreen;
    
    public event Action<bool> OnBattleOver;
    
    BattleState state;
    int currentAction;
    int currentMove;
    int currentMember;

    PlayerParty playerParty;
    Enemy wildEnemy;
    
    public void StartBattle(PlayerParty playerParty, Enemy wildEnemy)
    {
        this.playerParty = playerParty;
        this.wildEnemy = wildEnemy;
        
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.Setup(playerParty.GetHealthyMember());
        enemyUnit.Setup(wildEnemy);

        partyScreen.Init();
        
        dialogBox.SetMoveNames(playerUnit.Enemy.Moves);

        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Enemy.Base.Name} appeared.");

        ChooseFirstTurn();
    }
    
    void ChooseFirstTurn()
    {
        if (playerUnit.Enemy.Speed >= enemyUnit.Enemy.Speed)
            ActionSelection();
        else
            StartCoroutine(EnemyMove());
    }

    void BattleOver(bool won)
    {
        state = BattleState.BattleOver;
        playerParty.Enemies.ForEach(p => p.OnBattleOver());
        OnBattleOver(won);
    }
    
    void ActionSelection()
    {
        state = BattleState.ActionSelection;
        dialogBox.SetDialog("Choose an action");
        dialogBox.EnableActionSelector(true);
    }
    
    void OpenPartyScreen()
    {
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Enemies);
        partyScreen.gameObject.SetActive(true);
    }

    void MoveSelection()
    {
        state = BattleState.MoveSelection;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    IEnumerator PlayerMove()
    {
        state = BattleState.PerformMove;

        var move = playerUnit.Enemy.Moves[currentMove];
        yield return RunMove(playerUnit, enemyUnit, move);

        //If the battle state wasn't changed by RunMove, then go to next step
        if (state == BattleState.PerformMove)
        {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.PerformMove;

        var move = enemyUnit.Enemy.GetRandomMove();
        yield return RunMove(enemyUnit, playerUnit, move);

        //If the battle state wasn't changed by RunMove, then go to next step
        if (state == BattleState.PerformMove)
        {
            ActionSelection();
        }
    }

    IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move)
    {
        bool canRunMove = sourceUnit.Enemy.OnBeforeMove();
        if (!canRunMove)
        {
            yield return ShowStatusChanges(sourceUnit.Enemy);
            yield return sourceUnit.Hud.UpdateHP();
            yield break;
        }
        
        move.PP--;
        yield return dialogBox.TypeDialog($"{sourceUnit.Enemy.Base.Name} used {move.Base.Name}");

        if (CheckIfMoveHits(move, sourceUnit.Enemy, targetUnit.Enemy))
        {

            sourceUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(1f);
            targetUnit.PlayHitAnimation();

            if (move.Base.Category == MoveCategory.Status)
            {
                yield return RunMoveEffects(move.Base.Effects, sourceUnit.Enemy, targetUnit.Enemy, move.Base.Target);
            }
            else
            {
                var damageDetails = targetUnit.Enemy.TakeDamage(move, sourceUnit.Enemy);
                yield return targetUnit.Hud.UpdateHP();
                yield return ShowDamageDetails(damageDetails);
            }
            
            if (move.Base.Secondaries != null && move.Base.Secondaries.Count > 0 && targetUnit.Enemy.HP > 0)
            {
                foreach (var secondary in move.Base.Secondaries)
                {
                    var rnd = UnityEngine.Random.Range(1, 101);
                    if (rnd <= secondary.Chance)
                        yield return RunMoveEffects(secondary, sourceUnit.Enemy, targetUnit.Enemy, secondary.Target);
                }
            }

            if (targetUnit.Enemy.HP <= 0)
            {
                yield return dialogBox.TypeDialog($"{targetUnit.Enemy.Base.Name} Fainted");
                targetUnit.PlayFaintAnimation();

                yield return new WaitForSeconds(2f);

                CheckForBattleOver(targetUnit);
            }
        }
        else
        {
            yield return dialogBox.TypeDialog($"{sourceUnit.Enemy.Base.Name}'s attack missed");
        }

        //Status like burn or COVID will hurt player/enemy after turn
        sourceUnit.Enemy.OnAfterTurn();
        yield return ShowStatusChanges(sourceUnit.Enemy);
        yield return sourceUnit.Hud.UpdateHP();
        if (sourceUnit.Enemy.HP <= 0)
        {
            yield return dialogBox.TypeDialog($"{targetUnit.Enemy.Base.Name} Fainted");
            sourceUnit.PlayFaintAnimation();
            
            yield return new WaitForSeconds(2f);
            
            CheckForBattleOver(sourceUnit);
        }
    }
    
    IEnumerator RunMoveEffects(MoveEffects effects, Enemy source, Enemy target, MoveTarget moveTarget)
    {
        // Stat Boosting
        if (effects.Boosts != null)
        {
            if (moveTarget == MoveTarget.Self)
                source.ApplyBoosts(effects.Boosts);
            else
                target.ApplyBoosts(effects.Boosts);
        }
        
        // Status Condition
        if (effects.Status != ConditionID.none)
        {
            target.SetStatus(effects.Status);
        }
        
        // Volatile Status Condition
        if (effects.VolatileStatus != ConditionID.none)
        {
            target.SetVolatileStatus(effects.VolatileStatus);
        }

        yield return ShowStatusChanges(source);
        yield return ShowStatusChanges(target);
    }
    
    bool CheckIfMoveHits(Move move, Enemy source, Enemy target)
    {
        if (move.Base.AlwaysHits)
            return true;

        float moveAccuracy = move.Base.Accuracy;

        int accuracy = source.StatBoosts[Stat.Accuracy];
        int evasion = target.StatBoosts[Stat.Evasion];

        var boostValues = new float[] { 1f, 4f / 3f, 5f / 3f, 2f, 7f / 3f, 8f / 3f, 3f };

        if (accuracy > 0)
            moveAccuracy *= boostValues[accuracy];
        else
            moveAccuracy /= boostValues[-accuracy];

        if (evasion > 0)
            moveAccuracy /= boostValues[evasion];
        else
            moveAccuracy *= boostValues[-evasion];

        return UnityEngine.Random.Range(1, 101) <= moveAccuracy;
    }
    
    IEnumerator ShowStatusChanges(Enemy enemy)
    {
        while (enemy.StatusChanges.Count > 0)
        {
            var message = enemy.StatusChanges.Dequeue();
            yield return dialogBox.TypeDialog(message);
        }
    }

    void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if (faintedUnit.IsPlayerUnit)
        {
            var nextPartyMember = playerParty.GetHealthyMember();
            if (nextPartyMember != null)
            {
                OpenPartyScreen();
            }
            else
            {
                BattleOver(false);
            }
        }
        else
        {
            BattleOver(true);
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1f)
            yield return dialogBox.TypeDialog("A critical hit!");

        if (damageDetails.TypeEffectiveness > 1f)
            yield return dialogBox.TypeDialog("It's super effective!");
        else if (damageDetails.TypeEffectiveness < 1f)
            yield return dialogBox.TypeDialog("It's not very effective...");
    }

    public void HandleUpdate()
    {
        if (state == BattleState.ActionSelection)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.MoveSelection)
        {
            HandleMoveSelection();
        }
        else if (state == BattleState.PartyScreen)
        {
            HandlePartySelection();
        }
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentAction;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentAction;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentAction += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentAction -= 2;

        currentAction = Mathf.Clamp(currentAction, 0, 3);

        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                // Fight
                MoveSelection();
            }
            else if (currentAction == 1)
            {
                // Bag
            }
            else if (currentAction == 2)
            {
                // Party
                OpenPartyScreen();
            }
            else if (currentAction == 3)
            {
                // Run
            }
        }
    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentMove;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentMove;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentMove += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentMove -= 2;

        currentMove = Mathf.Clamp(currentMove, 0, playerUnit.Enemy.Moves.Count - 1);

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Enemy.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PlayerMove());
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            ActionSelection();
        }
    }
    
    void HandlePartySelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentMember;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentMember;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentMember += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentMember -= 2;

        currentMember = Mathf.Clamp(currentMember, 0, playerParty.Enemies.Count - 1);

        partyScreen.UpdateMemberSelection(currentMember);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var selectedMember = playerParty.Enemies[currentMember];
            if (selectedMember.HP <= 0)
            {
                partyScreen.SetMessageText("A voice whispered: They've fainted, they can't come out.");
                return;
            }
            if (selectedMember == playerUnit.Enemy)
            {
                partyScreen.SetMessageText("A voice whispered: They're already on the battlefield!");
                return;
            }

            partyScreen.gameObject.SetActive(false);
            state = BattleState.Busy;
            StartCoroutine(SwitchPartyMember(selectedMember));
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            partyScreen.gameObject.SetActive(false);
            ActionSelection();
        }
    }

    IEnumerator SwitchPartyMember(Enemy newMember)
    {
        bool currentMemberFainted = true;
        if (playerUnit.Enemy.HP > 0)
        {
            currentMemberFainted = false;
            yield return dialogBox.TypeDialog($"{playerUnit.Enemy.Base.Name}: I'm getting out of here!");
            playerUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(2f);
        }

        playerUnit.Setup(newMember);
        dialogBox.SetMoveNames(newMember.Moves);
        yield return dialogBox.TypeDialog($"{newMember.Base.Name}: I'm ready!");
        if (currentMemberFainted)
            ChooseFirstTurn();
        else
            StartCoroutine(EnemyMove());
    }
}