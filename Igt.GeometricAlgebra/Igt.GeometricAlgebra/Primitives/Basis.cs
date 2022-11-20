using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace Igt.GeometricAlgebra.Primitives
{
	/// <summary>
	/// This is the most fundamental object which allows for a 
	/// </summary>
	public struct Basis : IBasis
	{
		static readonly uint[] BITMASK_LOOKUP = new uint[]
		{
		0x00000001, 0x00000002, 0x00000004, 0x00000008, 0x00000010, 0x00000020, 0x00000040, 0x00000080,
		0x00000100, 0x00000200, 0x00000400, 0x00000800, 0x00001000, 0x00002000, 0x00004000, 0x00008000,
		0x00010000, 0x00020000, 0x00040000, 0x00080000, 0x00100000, 0x00200000, 0x00400000, 0x00800000,
		0x01000000, 0x02000000, 0x04000000, 0x08000000, 0x10000000, 0x20000000, 0x40000000, 0x80000000
		};

		static readonly uint[] BITMASK_XOR_LOOKUP = new uint[]
		{
		0xFFFFFFFE, 0xFFFFFFFD, 0xFFFFFFFB, 0xFFFFFFF7, 0xFFFFFFEF, 0xFFFFFFDF, 0xFFFFFFBF, 0xFFFFFF7F,
		0xFFFFFEFF, 0xFFFFFDFF, 0xFFFFFBFF, 0xFFFFF7FF, 0xFFFFEFFF, 0xFFFFDFFF, 0xFFFFBFFF, 0xFFFF7FFF,
		0xFFFEFFFF, 0xFFFDFFFF, 0xFFFBFFFF, 0xFFF7FFFF, 0xFFEFFFFF, 0xFFDFFFFF, 0xFFBFFFFF, 0xFF7FFFFF,
		0xFEFFFFFF, 0xFDFFFFFF, 0xFBFFFFFF, 0xF7FFFFFF, 0xEFFFFFFF, 0xDFFFFFFF, 0xBFFFFFFF, 0x7FFFFFFF
		};

		static readonly ReadOnlyDictionary<string, uint> NAMED_BASIS_LOOKUP = new Dictionary<string, uint>()
		{
			{ "S", S },
			{ "PS", PS },
			{ "e1", e1 },
			{ "e2", e2 },
			{ "e3", e3 },
			{ "e12", e12 },
			{ "e23", e23 },
			{ "e31", e31 },
			{ "e123", e123 },
		}.AsReadOnly();

		public static readonly Basis S = new(0);
		public static readonly Basis PS = new(0x1F);

		public static readonly Basis e1 = new(0x1);
		public static readonly Basis e2 = new(0x1 << 1);
		public static readonly Basis e3 = new(0x1 << 2);

		public static readonly Basis e12 = new(e1 | e2);
		public static readonly Basis e23 = new(e2 | e3);
		public static readonly Basis e31 = new(e3 | e1);

		public static readonly Basis e123 = new(e1 | e2 | e3);

		private int grade;

		/// <summary>
		/// A bitmask which stores which basis elements are required to construct this <see cref="Basis"/>
		/// </summary>
		public uint BitMask { get; set; }
		/// <summary>
		/// Coefficient which simply controls the sign and Weight of the <see cref="Basis"/>
		/// </summary>
		public float Scale { get; set; }
		public int Grade => grade;



		/// <summary>
		/// Instantiate a new <see cref="Basis"/> instance. This is the most fundamental object which
		/// defines every other object as a composition of multiple instances of <see cref="Basis"/>
		/// and the <see cref="IOperation{IBasisBlade}"/> implementations.
		/// </summary>
		/// <param name="bitmap"></param>
		/// <param name="scale"></param>
		public Basis(uint bitmap = default, float scale = 0)
		{
			BitMask = bitmap;
			Scale = scale;

			grade = 0;
			var temp = bitmap;
			while (temp != 0)
			{
				grade++;
				temp >>= 1;
			}
		}

		public static implicit operator uint(Basis x)
		{
			return x.BitMask;
		}

		public static Basis operator |(Basis x, Basis y)
		{
			return new(x.BitMask | y.BitMask);
		}

		public bool this[string basis]
		{
			get =>
				NAMED_BASIS_LOOKUP.TryGetValue(basis, out var index)
					? this[index]
					: throw new IndexOutOfRangeException("Basis does not currently support a linear space of more than 32 dimensions");

			set
			{
				if (NAMED_BASIS_LOOKUP.TryGetValue(basis, out var index))
					this[index] = value;
				else
					throw new IndexOutOfRangeException("Basis does not currently support a linear space of more than 32 dimensions");
			}
		}

		public bool this[int index]
		{
			get => this[(uint)index];

			set => this[(uint)index] = value;
		}

		public bool this[uint index]
		{
			get =>
			  index is >= 32 or < 0
				? throw new IndexOutOfRangeException("Basis does not currently support a linear space of more than 32 dimensions")
				: GetElement(index);

			set
			{
				if (index is >= 32 or < 0)
				{
					throw new IndexOutOfRangeException("Basis does not currently support a linear space of  more than 32 dimensions");
				}
				else
				{
					SetElement(index, value);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SetElement(uint index, bool value)
		{
			if (value)
			{
				BitMask |= BITMASK_LOOKUP[index];
			}
			else
			{
				BitMask &= BITMASK_XOR_LOOKUP[index];
			}
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool GetElement(uint index) => (BitMask & BITMASK_LOOKUP[index]) == 1;
	}
}
