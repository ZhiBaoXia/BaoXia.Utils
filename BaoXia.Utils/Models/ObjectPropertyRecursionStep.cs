using System.Collections.Generic;

namespace BaoXia.Utils.Models;

public class ObjectPropertyRecursionStep : RecursionStep<ObjectPropertyInfo>
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public object? ParentEntity { get; set; }

	public object? CurrentEntityPropertyValue { get; set; }

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public ObjectPropertyRecursionStep()
		: base(null, [], 0)
	{ }

	public ObjectPropertyRecursionStep(
	    ObjectPropertyInfo? parentItem,
	    IList<ObjectPropertyInfo> steps,
	    int nextStepIndex)
	    : base(parentItem, steps, nextStepIndex)
	{ }

	#endregion
}