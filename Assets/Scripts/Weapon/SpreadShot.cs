using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShot : Weapon
{
    [SerializeField]
    int numberOfBullets;
    [SerializeField]
    float spreadAngle;
    [SerializeField]
    Projectile projectilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Fire(Transform shootpoint, float range, bool isPlayer)
    {
        base.Fire(shootpoint, range, isPlayer);

        StartCoroutine(ShootProjectiles(shootpoint, range));
        
    }
    IEnumerator ShootProjectiles(Transform shootpoint, float range)
    {
        //TODO Spreadshote audio
        canFire = false;
        if (numberOfBullets > 0)
        {
            float offset = spreadAngle / numberOfBullets;
            float gunAngle = shootpoint.transform.rotation.eulerAngles.z;
            float startAngle = gunAngle - ((numberOfBullets / 2) * offset);
            for (int i = 0; i < numberOfBullets; i++)
            {
                Instantiate(projectilePrefab, shootpoint.transform.position, Quaternion.Euler(new Vector3(0f, 0f, startAngle)));
                startAngle += offset;
            }
        }
        yield return new WaitForSeconds(coolDown);
        canFire = true;
    }
}
