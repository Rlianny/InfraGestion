namespace Domain.Enums
{
    public enum DecommissioningReason
    {
        IrreparableTechnicalFailure,
        TechnologicalObsolescence,
        EOL,
        ExcessiveRepairCost,
        SeverePhysicalDamage,
        IncompatibilityWithNewInfrastructure,
        PlannedTechnologyUpgrade,
        TheftOrLoss,
        CustomerContractTermination
    }
}