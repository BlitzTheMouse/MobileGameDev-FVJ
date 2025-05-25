using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEndBehaviour : MonoBehaviour
{
    public float destroyTime = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerBehaviour>())
        {
            var gm = GameObject.FindObjectOfType<GameManager>();
            gm.SpawnNextTile();

            Destroy(transform.parent.gameObject,destroyTime);
        }
    }
}
