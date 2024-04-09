using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class Control : MonoBehaviour
{
    public Fuck[] fuck = new Fuck[3];
    public Fuck[] fuck1 = new Fuck[3];

    public GameObject pivot;
    public float distance;

    public float[] nmsl = new float[3];
    public float[] nmsl1 = new float[3];

    public void Start()
    {
        Init();
        Move();
    }
    public void Init()
    {
        for (int i = 0; i < fuck.Length; i++)
        {
            nmsl[i] = distance * (i + 0.5f);
            if (!fuck[i]) continue;
            fuck[i].transform.position = pivot.transform.position + new Vector3 (distance * (i+0.5f), 0f, 0f);
            fuck[i].onFightEnd += () => Move();
        }
        for (int i = 0; i < fuck1.Length; i++)
        {
            nmsl1[i] = -distance * (i + 0.5f);
            if (!fuck1[i]) continue;
            fuck1[i].transform.position = pivot.transform.position + new Vector3(-distance * (i + 0.5f), 0f, 0f);
        }
    }

    public void Fight()
    {
        if (fuck[0].died || fuck1[0].died)
        {
            Debug.Log("dawanle");
            return;
        }
        float damage = Mathf.Min(fuck[0].HP, fuck1[0].HP);
        fuck[0].onDamaged += () => fuck[0].HP = fuck[0].HP- damage;
        fuck1[0].onDamaged += () => fuck1[0].HP = fuck1[0].HP - damage;

        Debug.Log(damage);
        fuck[0].PlayFight(false);
        fuck1[0].PlayFight(true);
    }

    public void Move()
    {
        var fuckme = fuck;
        var nm = nmsl;
        Fuck lastone = null;
        fuckme = fuck1;
        nm = nmsl1;
        int target = -1;
        for (int i = 0; i < fuckme.Length; i++)
        {
            if (!fuckme[i] || fuckme[i].died)
            {
                target = i;
            }
            else
            {
                if (i == 0 || target == -1)
                    continue;

                lastone = fuckme[i];
                fuckme[i].onMoveEnd = null;
                fuckme[i].Move(new Vector3(nm[target], 0, 0));
                fuckme[target] = fuckme[i];
                fuckme[i] = null;
                i = target;
            }
        }
        fuckme = fuck;
        nm = nmsl;
        target = -1;
        for (int i = 0; i < fuckme.Length; i++)
        {
            if (!fuckme[i] || fuckme[i].died)
            {
                target = i;
            }
            else
            {
                if (i == 0 || target == -1)
                    continue;

                lastone = fuckme[i];
                fuckme[i].onMoveEnd = null;
                fuckme[i].Move(new Vector3(nm[target], 0, 0));
                fuckme[target] = fuckme[i];
                fuckme[i] = null;
                i = target;
            }
        }

        if (lastone != null)
            lastone.onMoveEnd += Fight;
        else
            Fight();
    }
}
