public interface IChangeTrack
{
    void IndicatorTrack(float indicatorPosition, float center, float halfWidth);
    void MovementSense(bool forward);

    void NoiseWithoutTrack();
}