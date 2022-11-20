using System;
using MathNet.Numerics.LinearAlgebra;

namespace Igt.GeometricAlgebra.Primitives;

public record struct Metric<T> where T : IEquatable<T>, new()
{
  public Metric(T[] elements)
  {
	Elements = elements;
	ScalarType = typeof(T);
	Grade = (uint)elements.Length;
  }

  public Type ScalarType { get; init; }
  public uint Grade { get; }
  public bool IsEuclidian { get; }
  public bool IsAntiEuclidian { get; }
  public bool IsDiagonal { get; }
  public T[] Elements { get; init; }

  public Matrix<uint> GetInnerProductMatrix()
  {
	var builder = CreateMatrix.Dense<uint>(1, 4);

	return builder;
  }
}
