
using UnityEngine;



public enum ExecutionMode
{
    Store,
    Museum
}
public enum StoreSubMode
{
    FullExperience,
    AllRunesActivated
}

[CreateAssetMenu(menuName = "Execution Settings", fileName = "ExecutionSettings")]
public class ExecutionSettings : ScriptableObject
{
    public ExecutionMode mode = ExecutionMode.Store;
    [Header("Configuración de submodo Store")]
    public StoreSubMode storeSubMode = StoreSubMode.FullExperience;
}
