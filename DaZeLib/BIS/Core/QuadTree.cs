using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BIS.Core
{
  public class QuadTree<TElement> : IEnumerable<TElement>, IEnumerable
  {
    private int sizeX;
    private int sizeY;
    private int sizeTotalX;
    private int sizeTotalY;
    private const int logSizeX = 2;
    private const int logSizeY = 2;
    private int logSizeTotalX;
    private int logSizeTotalY;
    private static int leafLogSizeX;
    private static int leafLogSizeY;
    private QuadTree<TElement>.IQuadTreeNode root;
    private bool flag;
    private IEnumerable<TElement> allElementsEnumeration;
    private static Func<byte[], int, TElement> readElement;
    private static int elementSize;

    public int SizeX => this.sizeX;

    public int SizeY => this.sizeY;

    public QuadTree(
      int sizeX,
      int sizeY,
      BinaryReader input,
      Func<byte[], int, TElement> readElement,
      int elementSize)
    {
      QuadTree<TElement>.readElement = readElement;
      QuadTree<TElement>.elementSize = elementSize;
      this.CalculateDimensions(sizeX, sizeY);
      this.allElementsEnumeration = Enumerable.Range(0, this.SizeY).SelectMany<int, int, TElement>((Func<int, IEnumerable<int>>) (y => Enumerable.Range(0, this.SizeX)), (Func<int, int, TElement>) ((y, x) => this.Get(x, y)));
      this.flag = input.ReadBoolean();
      if (this.flag)
        this.root = (QuadTree<TElement>.IQuadTreeNode) new QuadTree<TElement>.QuadTreeNode(input);
      else
        this.root = (QuadTree<TElement>.IQuadTreeNode) new QuadTree<TElement>.QuadTreeLeaf(input);
    }

    public TElement Get(int x, int y)
    {
      if (x < 0 || x >= this.sizeX)
        throw new ArgumentOutOfRangeException(nameof (x));
      if (y < 0 || y >= this.sizeY)
        throw new ArgumentOutOfRangeException(nameof (y));
      uint shiftedX = (uint) (x << 32 - this.logSizeTotalX);
      uint shiftedY = (uint) (y << 32 - this.logSizeTotalY);
      return this.flag ? ((QuadTree<TElement>.QuadTreeNode) this.root).Get(x, y, shiftedX, shiftedY) : ((QuadTree<TElement>.QuadTreeLeaf) this.root).Get(x, y);
    }

    private void CalculateDimensions(int x, int y)
    {
      this.sizeX = x;
      this.sizeY = y;
      --x;
      this.logSizeTotalX = 0;
      for (; x != 0; x >>= 1)
        ++this.logSizeTotalX;
      --y;
      this.logSizeTotalY = 0;
      for (; y != 0; y >>= 1)
        ++this.logSizeTotalY;
      switch (QuadTree<TElement>.elementSize)
      {
        case 1:
          QuadTree<TElement>.leafLogSizeX = 1;
          QuadTree<TElement>.leafLogSizeY = 1;
          break;
        case 2:
          QuadTree<TElement>.leafLogSizeX = 1;
          QuadTree<TElement>.leafLogSizeY = 0;
          break;
        case 4:
          QuadTree<TElement>.leafLogSizeX = 0;
          QuadTree<TElement>.leafLogSizeY = 0;
          break;
        default:
          throw new ArgumentException("Element size needs to be 1, 2 or 4");
      }
      int num1 = (this.logSizeTotalX - QuadTree<TElement>.leafLogSizeX + 2 - 1) / 2;
      int num2 = (this.logSizeTotalY - QuadTree<TElement>.leafLogSizeY + 2 - 1) / 2;
      int num3 = num1 > num2 ? num1 : num2;
      this.logSizeTotalX = num3 * 2 + QuadTree<TElement>.leafLogSizeX;
      this.logSizeTotalY = num3 * 2 + QuadTree<TElement>.leafLogSizeY;
      this.sizeTotalX = 1 << this.logSizeTotalX;
      this.sizeTotalY = 1 << this.logSizeTotalY;
      Debug.Assert(this.sizeTotalX >= this.sizeX);
      Debug.Assert(this.sizeTotalY >= this.sizeY);
    }

    public IEnumerator<TElement> GetEnumerator() => this.allElementsEnumeration.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.allElementsEnumeration.GetEnumerator();

    private interface IQuadTreeNode
    {
    }

    private class QuadTreeNode : QuadTree<TElement>.IQuadTreeNode
    {
      private const int logSizeX = 2;
      private const int logSizeY = 2;
      private short flag;
      private QuadTree<TElement>.IQuadTreeNode[] subTrees = new QuadTree<TElement>.IQuadTreeNode[16];

      public QuadTreeNode(BinaryReader input) => this.Read(input);

      private void Read(BinaryReader input)
      {
        this.flag = input.ReadInt16();
        short flag = this.flag;
        for (int index = 0; index < 16; ++index)
        {
          this.subTrees[index] = ((int) flag & 1) != 1 ? (QuadTree<TElement>.IQuadTreeNode) new QuadTree<TElement>.QuadTreeLeaf(input) : (QuadTree<TElement>.IQuadTreeNode) new QuadTree<TElement>.QuadTreeNode(input);
          flag >>= 1;
        }
      }

      public TElement Get(int x, int y, uint shiftedX, uint shiftedY)
      {
        uint num1 = shiftedX >> 30;
        int index = ((int) (shiftedY >> 30) << 2) + (int) num1;
        if (((uint) this.flag & (uint) (1 << index)) > 0U)
          return ((QuadTree<TElement>.QuadTreeNode) this.subTrees[index]).Get(x, y, shiftedX << 2, shiftedY << 2);
        int num2 = (1 << QuadTree<TElement>.leafLogSizeX) - 1;
        int num3 = (1 << QuadTree<TElement>.leafLogSizeY) - 1;
        return ((QuadTree<TElement>.QuadTreeLeaf) this.subTrees[index]).Get(x & num2, y & num3);
      }
    }

    private class QuadTreeLeaf : QuadTree<TElement>.IQuadTreeNode
    {
      private byte[] value;
      private static Func<byte[], int, int, TElement> getFunc;

      public QuadTreeLeaf(BinaryReader input)
      {
        if (QuadTree<TElement>.QuadTreeLeaf.getFunc == null)
        {
          switch (QuadTree<TElement>.elementSize)
          {
            case 1:
              QuadTree<TElement>.QuadTreeLeaf.getFunc = (Func<byte[], int, int, TElement>) ((src, x, y) => QuadTree<TElement>.readElement(src, 0));
              break;
            case 2:
              QuadTree<TElement>.QuadTreeLeaf.getFunc = (Func<byte[], int, int, TElement>) ((src, x, y) => QuadTree<TElement>.readElement(src, x * 2));
              break;
            case 4:
              QuadTree<TElement>.QuadTreeLeaf.getFunc = (Func<byte[], int, int, TElement>) ((src, x, y) => QuadTree<TElement>.readElement(src, (y << 1) + x));
              break;
          }
        }
        this.value = input.ReadBytes(4);
      }

      public TElement Get(int x, int y) => QuadTree<TElement>.QuadTreeLeaf.getFunc(this.value, x, y);
    }
  }
}
