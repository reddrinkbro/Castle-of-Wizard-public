using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyCharacter;
    [SerializeField] private GameObject enemyCreatePosition;
    private List<EnemyCharacter> enemies;
    private bool isFindPlayer = false;
    private int enemyValue = 0;
    private float findTime = 0;
    // Use this for initialization
    void Start()
    {
        enemies = new List<EnemyCharacter>();
        InvokeRepeating("RandomEnemyCharacterMake", 0, 2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RandomEnemyCharacterMake()
    {
        if (enemyValue < 5)
        {
            int randomEnemyValue = Random.Range(0, enemyCharacter.Length);
            int randomPositionValue = Random.Range(1, enemyCreatePosition.transform.childCount - 1);
            GameObject tmpCivil = Instantiate(enemyCharacter[randomEnemyValue], enemyCreatePosition.transform.GetChild(randomPositionValue).position, Quaternion.identity);
            EnemyCharacter enemy = tmpCivil.GetComponent<EnemyCharacter>();
			enemy.SetEnemyCharacterManager(this);
			enemies.Add(enemy);
			enemyValue++;
            enemies[enemyValue - 1].Init(enemyCreatePosition, randomPositionValue);
        }
    }
	public void DecreaseEnemyCount(EnemyCharacter enemy)
	{
		enemyValue--;
		for (int i = 0; i < enemies.Count; i++)
		{
			if(enemies.Contains(enemy))
			{
				enemies.Remove(enemy);
				break;
			}
		}
	}
}
