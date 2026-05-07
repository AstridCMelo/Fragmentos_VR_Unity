public interface IChangeTrack
{
    void IndicatorTrack( float center, float halfWidth);
    void MovementSense(bool forward, float indicatorPosition);

    void NoiseWithoutTrack();
}