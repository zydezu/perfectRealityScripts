using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerClasses : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        enemy enemy = new enemy("TEST");
        //Console.WriteLine(enemy.level);
        playerCharacter p1 = new playerCharacter("AAA");
        playerCharacter p2 = new playerCharacter("BBB");
        //Console.WriteLine(enemy.charactername);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class item
{
    public string itemName;
    public int ID;
    public string type;
    public int attackDMG;
    public int salesCost;
    public int maxStacks;

    void sellItem()
    {
        //Console.WriteLine("Not done!");
    }
}

public class playerCharacter
{
    public string charactername;
    public int HP;
    public int maxHP;
    public int level = 0;
    public item item;
    public playerCharacter(string name)
    {
        charactername = name;
    }
}

public class enemy : playerCharacter
{
    public bool inBattle;
    public int attackDMG;
    public int dmgRandom;
    public int defence;
    new public int level = 11; //override level
    public int XPdrop;
    public enemy(string name) : base(name)
    {
        charactername = name;
    }
}
