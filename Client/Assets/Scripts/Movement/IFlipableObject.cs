public interface IFlipableObject
{
    /// <summary>
    /// ������Ʈ�� �ٶ󺸴� ����
    /// </summary>
    public int FacingDirection { get; }

    /// <summary>
    /// 2D ȸ��
    /// </summary>
    public abstract void Flip();
}