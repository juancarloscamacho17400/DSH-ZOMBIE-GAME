using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rounds : MonoBehaviour
{
    public int maxRound;
    public EnemyValues[] enemyRounds;
    public Text round;
    private EnemyValues currentRound;
    private float waitTime = 0;
    private int currentNumberRound = 0;
    private int enemiesLeft = 0;
    private int enemiesToKill = 0;

    // Start is called before the first frame update
    void Start()
    {
        NextRound();
        MovimientoBala.OnDeathAnother += killAnother;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesLeft > 0 && Time.time > waitTime)
        {
            this.enemiesLeft--;
            waitTime = Time.time + currentRound.enemyTime;
            GameObject newEnemy = Instantiate(currentRound.enemyType, transform.position, Quaternion.identity);
}
    }

    void killAnother()
    {
        enemiesToKill--;

        if(enemiesToKill == 0)
        {
            if(currentNumberRound < maxRound){
                NextRound();
            }
        }
    }

    void NextRound()
    {   
        currentNumberRound++;
        currentRound = enemyRounds[currentNumberRound - 1];
        enemiesLeft = currentRound.enemyNumber;
        enemiesToKill = currentRound.enemyNumber;
        round.text = currentNumberRound.ToString();
    }
}
