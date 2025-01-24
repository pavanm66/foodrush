using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(" in on trigger enter");
        if (collision.CompareTag("Player"))
        {
            Debug.Log(collision.gameObject.name);
            
        }
    }
}
