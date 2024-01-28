using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class IntroCinematica : MonoBehaviour
{
    public GameObject Mickey;
    public Transform MickeySpawnPos;
    public Transform MickeyPosition;

    public Transform FirstCameraPosition;
    public Transform SecondCameraPosition;

    public AudioSource audio;
    public GameObject mosca;

    private void Start()
    {
        DOTween.Init();
        StartCoroutine(Cinematic());
    }

    public void SkipIntro()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public IEnumerator Cinematic()
    {
        Mickey.transform.position = MickeySpawnPos.transform.position;
        Mickey.transform.eulerAngles = new Vector3(0, 90, 0);//128
        Mickey.GetComponent<Animator>().Play("Walking");

        Vector3 end = MickeyPosition.position;

        // speed should be 1 unit per second
        while (Mickey.transform.position != end)
        {
            Mickey.transform.position = Vector3.MoveTowards(Mickey.transform.position, end, 4 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        Mickey.transform.eulerAngles = new Vector3(0, 0, 0);//128
        Mickey.GetComponent<Animator>().Play("IddleNeutral");
        yield return new WaitForSeconds(1);
        audio.Play();
        yield return new WaitForSeconds(1);
        Mickey.GetComponent<Animator>().Play("IddleTalk");
        yield return new WaitForSeconds(7);
        //Camera.main.transform.position = SecondCameraPosition.position;
        Camera.main.transform.DOMove(SecondCameraPosition.position, 2);
        yield return new WaitForSeconds(14);
        Camera.main.transform.DOMove(FirstCameraPosition.position, 2);
        mosca.SetActive(true);
        yield return new WaitForSeconds(10);

        SceneManager.LoadScene("Gameplay");
    }

}
