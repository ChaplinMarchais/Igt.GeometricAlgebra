namespace Igt.GeometricAlgebra.Primitives;

public interface IBasis
{
	bool this[uint index] { get; set; }
	bool this[int index] { get; set; }
	bool this[string index] { get; set; }

	uint BitMask { get; set; }
	float Scale { get; set; }
}