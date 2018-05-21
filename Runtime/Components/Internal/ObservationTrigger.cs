namespace DifferentMethods.Univents
{
    [System.Flags]
    public enum ObservationTrigger
    {
        None = 0,
        Update = 1 << 0,
        OnTriggerEnter = 1 << 1,
        OnTriggerStay = 1 << 2,
        OnTriggerExit = 1 << 3,
        OnCollisionEnter = 1 << 4,
        OnCollisionStay = 1 << 5,
        OnCollisionExit = 1 << 6,
        OnLateUpdate = 1 << 7,
        OnPointerEnter = 1 << 8,
        OnPointerClick = 1 << 9,
        OnPointerExit = 1 << 10,
        LateUpdate = 1 << 11,
        FixedUpdate = 1 << 12
    }
}
