using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class InediaMovementConfig
    {
        public bool WeightRestrictionsHandlerIsActive { get; set; }
        public float WeakSlowdownIfLoadGreaterThanKg { get; set; }
        public float MediumSlowdownIfLoadGreaterThanKg { get; set; }
        public float StrongSlowdownIfLoadGreaterThanKg { get; set; }
        public float ExtremeSlowdownIfLoadGreaterThanKg { get; set; }
        public float ForcedWalkIfLoadGreaterThanKg { get; set; }
        public float MovementRestrictionIfLoadGreaterThanKg { get; set; }

        public bool SlopeRestrictionsHandlerIsActive { get; set; }
        public float WeakSlowdownIfSlopeGreaterThanDegree { get; set; }
        public float MediumSlowdownIfSlopeGreaterThanDegree { get; set; }
        public float StrongSlowdownIfSlopeGreaterThanDegree { get; set; }
        public float ExtremeSlowdownIfSlopeGreaterThanDegree { get; set; }

        public bool SurfaceRestrictionsHandlerIsActive = true;
        public int SurfaceRoadSpeedReduction { get; set; }
        public int SurfaceGrassSpeedReduction { get; set; }
        public int SurfaceGrassTallSpeedReduction { get; set; }
        public int SurfaceForestSpeedReduction { get; set; }
        public int SurfaceStoneSpeedReduction { get; set; }
        public int SurfaceSandSpeedReduction { get; set; }
        public int SurfaceWaterSpeedReduction { get; set; }
        public Dictionary<string, int> SurfaceFootwearSpeedImpact { get; set; }

        public bool ForcedWalkIfCharacterIsInBush { get; set; }
        public Dictionary<string, bool> BushClasses { get; set; }

        public bool CumulativeSpeedReductionIsActive { get; set; }

        public float SyberiaProjectStrengthImpactFrom { get; set; }
        public float SyberiaProjectStrengthImpactTo { get; set; }
    }
}
