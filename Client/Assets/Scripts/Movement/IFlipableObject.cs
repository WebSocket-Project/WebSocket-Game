public interface IFlipableObject
{
    /// <summary>
    /// 오브젝트가 바라보는 방향
    /// </summary>
    public int FacingDirection { get; }

    /// <summary>
    /// 2D 회전
    /// </summary>
    public abstract void Flip();
}