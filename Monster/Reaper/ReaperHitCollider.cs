using UnityEngine;

public class ReaperHitCollider : MonoBehaviour
{
    public ReaperData reaperData;

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            col.GetComponent<PlayerState>().HitPlayer(reaperData.atk, true);
        }
    }
}
