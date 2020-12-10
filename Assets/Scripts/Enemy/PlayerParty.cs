using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerParty : MonoBehaviour
{
    [SerializeField] List<Enemy> members;

    public List<Enemy> Enemies {
        get {
            return members;
        }
    }
    
    private void Start()
    {
        foreach (var member in members)
        {
            member.Init();
        }
    }

    public Enemy GetHealthyMember()
    {
        return members.Where(x => x.HP > 0).FirstOrDefault();
    }
    
}
