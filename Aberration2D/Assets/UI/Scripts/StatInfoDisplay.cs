using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatInfoDisplay : MonoBehaviour
{
    [SerializeField] GameObject cover;
    [SerializeField] GameObject inventory;
    [Space]
    [SerializeField] TextMeshProUGUI magSize;
    [SerializeField] TextMeshProUGUI magLeft;
    [SerializeField] TextMeshProUGUI damage;
    [SerializeField] TextMeshProUGUI reload;
    [SerializeField] TextMeshProUGUI range;
    [SerializeField] TextMeshProUGUI bulletSpeed;
    [SerializeField] TextMeshProUGUI selfHurt;
    [SerializeField] TextMeshProUGUI burst;
    [SerializeField] TextMeshProUGUI accuracy;
    [SerializeField] TextMeshProUGUI moveSpeed;
    [SerializeField] TextMeshProUGUI jumpForce;
    [SerializeField] TextMeshProUGUI rollSpeed;
    [SerializeField] TextMeshProUGUI lifeSteal;

    bool display;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            display = !display;
            if (display)
                inventory.GetComponent<InventoryDisplay>().CreateInventoryItems(Player.instance.GetComponent<PlayerItems>().items);
        }
        GetComponent<Animator>().SetBool("playerDisplay", display);
        cover.GetComponent<Animator>().SetBool("display", display);
        inventory.GetComponent<Animator>().SetBool("display", display);

        magSize.text = Player.instance.GetComponent<PlayerShoot>().magazine.ToString() + "/" + PlayerStats.instance.magazineSize.ToString();
        magLeft.text = Player.instance.GetComponent<PlayerShoot>().magazine.ToString();
        damage.text = PlayerStats.instance.damage.ToString();
        reload.text = PlayerStats.instance.reloadTime.ToString();
        range.text = PlayerStats.instance.range.ToString();
        bulletSpeed.text = PlayerStats.instance.bulletSpeed.ToString();
        selfHurt.text = PlayerStats.instance.selfDamage.ToString();
        burst.text = PlayerStats.instance.burstSize.ToString();
        accuracy.text = PlayerStats.instance.bloomAngle.ToString();
        moveSpeed.text = PlayerStats.instance.moveSpeed.ToString();
        jumpForce.text = PlayerStats.instance.jumpForce.ToString();
        rollSpeed.text = PlayerStats.instance.rollSpeed.ToString();
        lifeSteal.text = PlayerStats.instance.lifeSteal.ToString();
    }
}
