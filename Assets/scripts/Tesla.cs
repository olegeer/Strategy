using UnityEngine;
using System.Collections;
using System.Linq;
public class Tesla : MonoBehaviour
{
    [SerializeField] Transform zipper; // ������
    [SerializeField] float damage;
    WaitForSeconds waitForZipper;
    [SerializeField] float distance;
    Enemy enemy;
    private void Start()
    {
        waitForZipper = new WaitForSeconds(0.25f);
        distance *= 1000;
        InvokeRepeating("Damage", 0.5f, 0.5f);
    }
    public void Damage()
    {
        if (!Spawner.singleton.ContainEnemy()) return;

        enemy = Spawner.singleton.enemies
            
            .Where(e => e != null)
            .Where(e => (e.transform.position - transform.position).sqrMagnitude < distance)
            .OrderBy(e => (e.transform.position - transform.position).sqrMagnitude).FirstOrDefault();

        print(enemy);
        if (enemy == null || !enemy.gameObject.activeSelf)
        {
            zipper.gameObject.SetActive(false);
            return;
        }

        zipper.LookAt(enemy.transform);
        SetZipper(transform.position, enemy.transform.position);
        //zipper.transform.localEulerAngles = new Vector3(0, 0, zipper.transform.localEulerAngles.z);
        zipper.gameObject.SetActive(true);
        enemy.GetDamage(damage);
        if (!gameObject.activeSelf) return;
        StartCoroutine("ZipperDisable");
    }
    void SetZipper(Vector2 startPos, Vector2 endPos)
    {
        float scale = Mathf.Abs(startPos.x - endPos.x);
        zipper.transform.localScale = new Vector3(1, 1, scale/262);
    }
    IEnumerator ZipperDisable()
    {
        if (!gameObject.activeSelf) yield break;
        yield return waitForZipper;
        zipper.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (enemy != null && enemy.gameObject.activeSelf && zipper.gameObject.activeSelf) SetZipper(transform.position, enemy.transform.position);
    }
}
