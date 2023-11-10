using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.UI;

public class SoldierController : MonoBehaviour
{
    enum SoldierState { inQueue, inWar, Dead }


    [SerializeField] private Transform[] moneyPopUpSpots;
    private Animator anim;
    public Soldier soldier;
    public PathCreator queuePath;
    float distancetravelled;
    [SerializeField] private int queueNumber;
    private SoldierState state;
    private FortController soldierfort;
    private Transform targetPos;
    private Transform eye;
    private Transform rightArm;
    private GameObject equippedArmor;
    private GameObject equippedSword;
    [SerializeField] private float health;
    private float maxHealth;
    private int moveSpeed;
    [SerializeField] private float damage;
    private GameObject enemyFromForward;
    private int fortId;
    public Image healthBar;
    private float gainMoneyValue;


    private void OnEnable()
    {

        moneyPopUpSpots = new Transform[transform.GetChild(2).childCount];
        maxHealth = soldier.health;
        health = maxHealth;
        damage = soldier.damage;
        moveSpeed = 0;
        healthBar.fillAmount = health / maxHealth;
        anim = transform.GetComponent<Animator>();
        state = SoldierState.inQueue;
        rightArm = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0);
        eye = transform.GetChild(1);
        if (transform.CompareTag("PlayerSoldier"))
        {
            queuePath = GameManager.Instance.GetPlayerPathObject().GetComponent<PathCreator>();
            soldierfort = GameManager.Instance.GetPlayerFortObject().GetComponent<FortController>();
            fortId = 1;
            for (int i = 0; i < transform.GetChild(2).childCount; i++)
            {
                moneyPopUpSpots[i] = transform.GetChild(2).GetChild(i);
            }


        }
        if (transform.CompareTag("EnemySoldier"))
        {
            queuePath = GameManager.Instance.GetEnemyPathObject().GetComponent<PathCreator>();
            soldierfort = GameManager.Instance.GetEnemyFortObject().GetComponent<FortController>();
            fortId = 2;

        }

        StartCoroutine("GetInGame");

    }
    IEnumerator GetInGame()
    {



        while (state != SoldierState.Dead && !GameManager.Instance.IsGameOver())
        {
            if (GameManager.Instance.IsGameGoing())
            {
                anim.SetInteger("moveSpeed", moveSpeed);
                targetPos = soldierfort.GetQueuePoint(queueNumber);
                distancetravelled += moveSpeed * Time.deltaTime;
                transform.position = queuePath.path.GetPointAtDistance(distancetravelled);
                transform.rotation = queuePath.path.GetRotationAtDistance(distancetravelled);

                if (state == SoldierState.inQueue)
                {
                    if (Vector3.Distance(transform.position, targetPos.position) > 0.4f) moveSpeed = 2;
                    else moveSpeed = 0;

                }

                if (state == SoldierState.inWar)
                {

                    Ray ray = new Ray(eye.position, eye.TransformDirection(Vector3.forward));
                    Debug.DrawRay(eye.position, eye.TransformDirection(Vector3.forward) * 3f, Color.red);
                    if (Physics.Raycast(ray, out RaycastHit hitInfo, 3f))
                    {

                        enemyFromForward = hitInfo.transform.gameObject;

                        if (transform.CompareTag("PlayerSoldier"))
                        {
                            if (hitInfo.transform.CompareTag("EnemySoldier") || hitInfo.transform.CompareTag("EnemyFort"))
                            {
                                if(hitInfo.transform.CompareTag("EnemyFort"))
                                {
                                    var deadEffect = ObjectPooler.Instance.GetDeadEffectParticlesFromPool();
                                    if (deadEffect != null)
                                    {
                                        Debug.Log("Deadeffect");
                                        deadEffect.transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
                                        deadEffect.SetActive(true);

                                    }
                                    GiveDamage();
                                    Destroy(gameObject);

                                }
                                moveSpeed = 0;


                                anim.SetBool("isAttacking", true);

                            }
                            if (hitInfo.transform.CompareTag("PlayerSoldier")) moveSpeed = 0;
                        }
                        if (transform.CompareTag("EnemySoldier"))
                        {
                            if (hitInfo.transform.CompareTag("PlayerSoldier") || hitInfo.transform.CompareTag("PlayerFort"))
                            {
                                if (hitInfo.transform.CompareTag("PlayerFort"))
                                {
                                    var deadEffect = ObjectPooler.Instance.GetDeadEffectParticlesFromPool();
                                    if (deadEffect != null)
                                    {
                                        Debug.Log("Deadeffect");
                                        deadEffect.transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
                                        deadEffect.SetActive(true);

                                    }
                                    GiveDamage();
                                    Destroy(gameObject);

                                }
                                moveSpeed = 0;


                                anim.SetBool("isAttacking", true);
                            }
                            if (hitInfo.transform.CompareTag("EnemySoldier")) moveSpeed = 0;
                        }
                    }
                    else
                    {
                        moveSpeed = soldier.moveSpeed;
                        anim.SetBool("isAttacking", false);
                    }
                    if (health <= 1)
                    {
                        if (transform.CompareTag("EnemySoldier"))
                        {
                            if (GameManager.Instance.GetPlayerLevel() == 1 && !TutorialManager.Instance.GetIs0LevelTutorialPlayed())
                            {
                                Ray rayTutorial = new Ray(eye.position, eye.TransformDirection(Vector3.forward));
                                Debug.DrawRay(eye.position, eye.TransformDirection(Vector3.forward) * 3f, Color.red);
                                if (Physics.Raycast(ray, out RaycastHit hitInfoTutorial, 3f))
                                {
                                    Destroy(hitInfoTutorial.transform.gameObject);
                                }
                                TutorialManager.Instance.SetFirstEnemySoldierState(true);
                                var deadEffect2 = ObjectPooler.Instance.GetDeadEffectParticlesFromPool();
                                if (deadEffect2 != null)
                                {
                                    deadEffect2.transform.position = transform.position;
                                    deadEffect2.SetActive(true);

                                }
                            }
                        }
                        var deadEffect = ObjectPooler.Instance.GetDeadEffectParticlesFromPool();
                        if (deadEffect != null)
                        {
                            Debug.Log("Deadeffect");
                            deadEffect.transform.position = new Vector3(transform.position.x, transform.position.y+5, transform.position.z);
                            deadEffect.SetActive(true);
                        
                        }
                        Destroy(gameObject);
                        yield return null;
                    }
                }
                yield return null;
            }
            else
            {
                anim.SetBool("isAttacking", false);
                moveSpeed = 0;
                yield return null;
            }

        }
        if (transform.CompareTag("PlayerSoldier") && GameManager.Instance.GetGameWinner() == GameManager.Winner.Player || transform.CompareTag("EnemySoldier") && GameManager.Instance.GetGameWinner() == GameManager.Winner.Enemy)
        {
            moveSpeed = 0;
            anim.SetInteger("moveSpeed", moveSpeed);
            anim.SetBool("isAttacking", false);
            //Debug.Log("victoryEmote");
        }
        else Destroy(gameObject);

        yield return null;

    }
    public void TakeUpArms(Item _item)
    {
        if (transform.CompareTag("PlayerSoldier"))
        {
            equipParticle();
            damage += _item.playerDamage;
            maxHealth += _item.playerHealth;


        }
        else
        {
            damage += _item.enemyDamage;
            maxHealth += _item.enemyHealth;

        }
        gainMoneyValue = _item.gainMoneyValue;
        health = maxHealth;
        healthBar.fillAmount = health / maxHealth;
        state = SoldierState.inWar;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag(_item.itemName))
            {
                transform.GetChild(i).gameObject.SetActive(true);
                equippedArmor = transform.GetChild(i).gameObject;

            }

        }
        for (int i = 0; i < rightArm.childCount; i++)
        {
            if (rightArm.GetChild(i).CompareTag(_item.itemName))
            {
                rightArm.GetChild(i).gameObject.SetActive(true);
                equippedSword = transform.GetChild(i).gameObject;
            }

        }

    }
    public void GiveDamage()
    {

        if (enemyFromForward != null)
        {
            if (enemyFromForward.GetComponent<FortController>() != null)
            {
                enemyFromForward.GetComponent<FortController>().TakeDamage(damage);
                

            }
            if (enemyFromForward.GetComponent<SoldierController>() != null)
            {
                enemyFromForward.GetComponent<SoldierController>().TakeDamage(damage);

            }

        }
        if (transform.CompareTag("PlayerSoldier"))
        {
            Vibrator.Vibrate(20);
            var moneyPopUp = ObjectPooler.Instance.GetMoneyPopUp();
            if (enemyFromForward.GetComponent<FortController>() != null)
            {
                if (moneyPopUp != null)
                {
                    moneyPopUp.transform.position = moneyPopUpSpots[Random.Range(0, 2)].position;
                    moneyPopUp.SetActive(true);
                    moneyPopUp.GetComponent<MoneyMove>().SetMoneyText(gainMoneyValue*3);

                }

            }
            if (enemyFromForward.GetComponent<SoldierController>() != null)
            {
                if (moneyPopUp != null)
                {
                    moneyPopUp.transform.position = moneyPopUpSpots[Random.Range(0, 2)].position;
                    moneyPopUp.SetActive(true);
                    moneyPopUp.GetComponent<MoneyMove>().SetMoneyText(gainMoneyValue);
                }
            }
        }
    }
    private void equipParticle()
    {
        Vibrator.Vibrate(50);
        var equipParticle = ObjectPooler.Instance.GetEquipParticlesFromPool();
        if (equipParticle != null)
        {
            equipParticle.transform.position = rightArm.transform.position;
            equipParticle.SetActive(true);

        }

    }
    public void SetQueueNumber(int value) => queueNumber = value;
    public void IncreaseQueueNumber() => queueNumber++;


    IEnumerator TakeDamageAnimated(float damage)
    {
        for (int i = 0; i < 3; i++)
        {
            health -= damage / 3;
            healthBar.fillAmount = health / maxHealth;
            yield return new WaitForSeconds(0.1f);

        }

    }
    public void TakeDamage(float damage)
    {
        int rand = Random.Range(1, 4);
        switch (rand)
        {
            case 1:
                AudioManager.Instance.PlaySFX2("HitSoldier1");
                break;
            case 2:
                AudioManager.Instance.PlaySFX2("HitSoldier2");
                break;
            case 3:
                AudioManager.Instance.PlaySFX2("HitSoldier3");
                break;
        }

        var particle = ObjectPooler.Instance.GetHitParticlesFromPool();
        if (particle != null)
        {
            particle.transform.position = transform.position;
            particle.SetActive(true);

        }
        StartCoroutine("TakeDamageAnimated", damage);


    }

    public int GetFortId() => fortId;

}
