using System.Collections.Generic;

namespace BaoXia.Utils.Models
{
	class RecursionStep<StepType>
	{
		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		public IList<StepType>? Steps { get; set; }

		public int NextStepIndex { get; set; }


		////////////////////////////////////////////////
		// @自身实现
		////////////////////////////////////////////////

		public RecursionStep()
		{ }

		public RecursionStep(
			IList<StepType> steps,
			int nextStepIndex)
		{
			this.Steps = steps;
			this.NextStepIndex = nextStepIndex;
		}

	}
}
