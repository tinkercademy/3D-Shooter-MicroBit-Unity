using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAiEffect : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;
    private DragonAi bossAI;

    [Header("Effects")]
    [SerializeField] ParticleSystem dirtEffect = default;
    [SerializeField] ParticleSystem waterEffect = default;
    ParticleSystem enterParticle, exitParticle;
    [SerializeField] LayerMask terrainLayer = default;

    void Start()
    {
        bossAI = GetComponent<DragonAi>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        //add the listener to the DragonAi.cs for the script in DragonAi to call it when needed
        bossAI.GroundContact.AddListener((boolA, boolB) => GroundContact(boolA, boolB));
        bossAI.GroundDetection.AddListener((x, y) => GroundParticleChange(x, y));

        //pre ray casting to get which effect to be played when the boss enter and leave the terrain (on water vs on ground)
        RaycastHit hitInfo;

        if (Physics.Raycast(bossAI.startPosition + new Vector3(0, 0.1f, 0), Vector3.down, out hitInfo, 1000, terrainLayer.value))
            enterParticle = hitInfo.transform.CompareTag("Terrain") ? dirtEffect : waterEffect;

        if (Physics.Raycast(bossAI.endPosition + new Vector3(0, 0.1f, 0), Vector3.down, out hitInfo, 1000, terrainLayer.value))
            exitParticle = hitInfo.transform.CompareTag("Terrain") ? dirtEffect : waterEffect;

    }

    void Update()
    { 
        //to trigger camera shake
        impulseSource.GenerateImpulse();
    }

    void GroundParticleChange(bool start, int particle)
    {
        //ray casting to get which effect to be played when the boss enter and leave the terrain (on water / on ground) function
        if (start)
            enterParticle = particle == 0 ? dirtEffect : waterEffect;
        else
            exitParticle = particle == 0 ? dirtEffect : waterEffect;
    }

    void GroundContact(bool state, bool start)
    {
        //generate/stop impluse(camera shake) when collided with the terrain layer (water/ground)
        if (start)
        {
            if (state)
            {
                enterParticle.transform.position = Vector3.Lerp(bossAI.startPosition, bossAI.endPosition, .1f);
                enterParticle.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
                enterParticle.Play();
            }
            else
            {
                enterParticle.Stop();
            }
        }
        else
        {
            if (state)
            {
                exitParticle.transform.position = Vector3.Lerp(bossAI.endPosition, bossAI.startPosition, .22f);
                exitParticle.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
                exitParticle.Play();
            }
            else
            {
                exitParticle.Stop();
            }
        }
    }
}
