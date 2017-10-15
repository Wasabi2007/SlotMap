using System;
using System.Threading;

namespace SlotMap
{
	public static class ArrayExtension
	{
		public static void Swap<T>(this T[] array, int index1, int index2)
		{
			T temp = array[index1];
			array[index1] = array[index2];
			array[index2] = temp;
		}
	}

	public class SlotMap<T>
	{
		public struct Key
		{
			/// <summary>
			/// Points to data inside the data Array
			/// If data slot is empty this points to the next free slot
			/// </summary>
			public int Index;
			public int Generation;
		}

		private int chunkSize = 16;
		private T[] data = new T[1];
		private Key[] indicies = new Key[1];
		private int[] erase = new int[1];

		private int capacity = 1;
		private int size;

		private int freeListHead = 0;
		private int freeListTail = 0;


		public SlotMap()
		{
			Expand();
			InitKeys(0);
		}

		public int Count
		{
			get { return size; }
		}

		private void InitKeys(int start)
		{
			for (int i = start; i < capacity; ++i)
			{
				indicies[i] = new Key() {Index = i+1};
			}
		}

		private void Expand()
		{

			var tailKey = indicies[freeListTail];
			tailKey.Index = capacity;
			indicies[freeListTail] = tailKey;

			capacity += chunkSize;
			freeListTail += chunkSize;

			Array.Resize(ref data, capacity);
			Array.Resize(ref indicies, capacity);
			Array.Resize(ref erase, capacity);

			InitKeys(capacity - chunkSize);
			chunkSize *= 2;
		}

		public T this[Key k]
		{
			get
			{
				var indexKey = indicies[k.Index];
				if(k.Generation != indexKey.Generation)
					return default(T);
				return data[indexKey.Index];
			}

			set
			{
				var indexKey = indicies[k.Index];
				if(k.Generation != indexKey.Generation)
					return;
				data[indexKey.Index] = value;
			}
		}

		public T this[int k]
		{
			get
			{
				return data[k];
			}
			set 
			{
				data[k] = value;
			}
		}

		public Key Add(T value)
		{
			if(freeListHead == freeListTail)
				Expand();

			var freeListHeadTemp = freeListHead;
			var key = indicies[freeListHeadTemp];
			key.Generation++;
			var returnKey = new Key() { Index = freeListHead , Generation = key.Generation};
			freeListHead = key.Index;

			key.Index = size;
			indicies[freeListHeadTemp] = key;

			data[key.Index] = value;
			erase[key.Index] = returnKey.Index;
			size++;
			return returnKey;
		}

		public void Remove(Key key)
		{
			var indexKey = indicies[key.Index];
			if(key.Generation != indexKey.Generation)
				return;

			data[indexKey.Index] = default(T);

			data.Swap(indexKey.Index, size - 1);
			erase.Swap(indexKey.Index, size - 1);

			int oldkeyIndex = erase[indexKey.Index];
			var movedDataKey = indicies[oldkeyIndex];
			movedDataKey.Index = indexKey.Index;
			indicies[oldkeyIndex] = movedDataKey;


			var tailKey = indicies[freeListTail];
			tailKey.Index = key.Index;
			indicies[freeListTail] = tailKey;

			freeListTail = key.Index;

			size--;
		}
	}
}