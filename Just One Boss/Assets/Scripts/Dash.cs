using UnityEngine;

public class Dash : MonoBehaviour
{
    private Spawner spawner;
    private Player player;
    private GameObject floatingTextObj;

    // Spawner에서 Init으로 참조를 넘겨줌

    private int displayScore;
    public void Init(Spawner spawnerRef, Player playerRef)
    {
        spawner = spawnerRef;
        player = playerRef;

        displayScore = (player.gameUI.GetCombo() + 1) * 100;

        floatingTextObj=FloatingTextManager.ShowText("+" + displayScore, transform.position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out Player p))
        {
            //Debug.Log("Player와 Dash 충돌");
            p.gameUI.AddCombo();
            p.gameUI.AddScore(displayScore);
            
            p.gameUI.AddRage(10);

            if (floatingTextObj != null)
            {
                floatingTextObj.GetComponent<FloatingText>().Play();
            }
            // 스폰 상태 false로 돌려놓음
            if (spawner != null)
                spawner.isDashSpawned = false;

            Destroy(gameObject);
        }
    }
}

