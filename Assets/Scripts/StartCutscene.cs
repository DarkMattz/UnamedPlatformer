using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCutscene : MonoBehaviour
{
    public Animator camAnim;
    public static bool isCutscene;
    [SerializeField] private bool useOnce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            isCutscene = true;
            PlayerBehavior.playerBody.velocity = new Vector2(0f, 0f);
            camAnim.SetBool("cutscene1", true);
            Invoke(nameof(stopCutscene), 4f);
        }
    }

    void stopCutscene()
    {
        isCutscene = false;
        camAnim.SetBool("cutscene1", false);
        if (useOnce)
            Destroy(this.gameObject);
    }

}
