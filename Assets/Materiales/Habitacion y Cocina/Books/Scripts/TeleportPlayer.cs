using UnityEngine;


/// <summary>
/// Manually teleport the player to a specific anchor
/// </summary>
public class TeleportPlayer : MonoBehaviour
{
    [Tooltip("The anchor the player is teleported to")]
    public UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationAnchor anchor = null;

    [Tooltip("The provider used to request the teleportation")]
    public UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider provider = null;

    void Start()
    {
        //Teleport(); // Teleport the player when the scene starts
    }

    public void Teleport(UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationAnchor anchors)
    {
        if(anchors && provider)
        {
            UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportRequest request = CreateRequest(anchors);
            provider.QueueTeleportRequest(request);
        }
    }

    private UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportRequest CreateRequest(UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationAnchor anchors)
    {
        Transform anchorTransform = anchors.teleportAnchorTransform;

        UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportRequest request = new UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportRequest()
        {
            requestTime = Time.time,
            matchOrientation = anchors.matchOrientation,

            destinationPosition = anchorTransform.position,
            destinationRotation = anchorTransform.rotation
        };

        return request;
    }
}
