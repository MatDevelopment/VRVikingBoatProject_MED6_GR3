using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This code is inspired by 
// https://www.youtube.com/watch?v=tPWMZ4Ic7PA&list=PLYuJT1wgJunlRwTk-q6EIL5kb3trRnFq0&index=9&t=157s&ab_channel=CodeMonkey
public class ChopDamage : MonoBehaviour, IDamageable
{
[SerializeField] private Transform pfLogBroken;
private HealthSystem healthSystem;
private Vector3 lastDamagePosition;
//public int ChoppedWood = 0;
public LevelChanger levelChanger;

public AudioSource WoodChop;

public AudioSource WoodSplit;

private void Awake()
{
    healthSystem = new HealthSystem(100);

    levelChanger = GameObject.FindWithTag("LevelChanger").GetComponent<LevelChanger>();
}

// Using update instead of event system used in video
void Update()
{
    if (healthSystem.health == 0)
        {
            Transform LogBrokenTransform = Instantiate(pfLogBroken, transform.position, transform.rotation); 
            foreach (Transform child in LogBrokenTransform)
                {
                    if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
                    {
                        childRigidbody.AddExplosionForce(100f, lastDamagePosition, 5f);
                    }
                }
            
            WoodSplit.Play();
            Destroy(gameObject);
        }

    if(levelChanger.countChoppedWood > 0 && levelChanger.WoodChopped == false)
    {
        //levelChanger.CountWooodChopped();
        levelChanger.WoodChopped = true;
    }
}

public void Damage(int damageAmount, Vector3 damagePosition)
{
    lastDamagePosition = damagePosition;
    healthSystem.Damage(damageAmount);
    WoodChop.Play();
}

public void IsChopped()
{
    levelChanger.countChoppedWood += 1;
}

}
