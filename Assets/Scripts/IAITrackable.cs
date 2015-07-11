using System;
using UnityEngine;

public interface IAITrackable
{
    float speed { get; set; }
    Transform trackingTransform { get; set; }
    Vector3 currentHeading { get; set; }
}
