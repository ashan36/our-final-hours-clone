using System;
using System.Collections.Generic;
using UnityEngine;

public class SightDetection
{
    public float sightRange { get; set; }
    public float sightAngle { get; set; }

    public SightDetection(float range, float angle)
    {
        sightRange = range;
        sightAngle = angle;
    }

    public bool Detect(GameObject objectOfInterest, Transform characterTrans)
    {
        Transform objectTrans = objectOfInterest.transform;
        RaycastHit rayHit = new RaycastHit();
        Vector3 rayDirection = objectTrans.position - characterTrans.position;

        if ((Vector3.Angle(rayDirection, characterTrans.forward)) < sightAngle)
        {
            if (Physics.Raycast(characterTrans.position, rayDirection, out rayHit, sightRange))
            {
                if (rayHit.collider.gameObject == objectOfInterest)
                    return true;
                else return false;
            }
            else return false;
        }
        else return false;
    }
}

public class SoundDetection
{
    public float hearingThreshold { get; set; }

    public SoundDetection(float threshold)
    {
        hearingThreshold = threshold;
    }

    public bool Detect(IEmitsSound soundSource, float distance)
    {
        float calculatedSoundLevel;
        if (soundSource.soundValue != 0)
        Debug.Log("Sound value at source: " + soundSource.soundValue);
        calculatedSoundLevel = soundSource.soundValue * (1 / (distance));
        if (calculatedSoundLevel != 0)
        Debug.Log("Calculated sound value at listener " + calculatedSoundLevel + " and hearing threshold is " + hearingThreshold);

        if (hearingThreshold <= calculatedSoundLevel)
            return true;
        else return false;
    }
}
