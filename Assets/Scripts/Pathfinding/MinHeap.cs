using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MinHeap
{
    private readonly Node[] _elements;
    private int _size;

    public MinHeap(int size)
    {
        _elements = new Node[size];
    }

    private int GetLeftChildIndex(int elementIndex) => 2 * elementIndex + 1;
    private int GetRightChildIndex(int elementIndex) => 2 * elementIndex + 2;
    private int GetParentIndex(int elementIndex) => (elementIndex - 1) / 2;

    private bool HasLeftChild(int elementIndex) => GetLeftChildIndex(elementIndex) < _size;
    private bool HasRightChild(int elementIndex) => GetRightChildIndex(elementIndex) < _size;
    private bool IsRoot(int elementIndex) => elementIndex == 0;

    private Node GetLeftChild(int elementIndex) => _elements[GetLeftChildIndex(elementIndex)];
    private Node GetRightChild(int elementIndex) => _elements[GetRightChildIndex(elementIndex)];
    private Node GetParent(int elementIndex) => _elements[GetParentIndex(elementIndex)];

    private void Swap(int firstIndex, int secondIndex)
    {
        var temp = _elements[firstIndex];
        _elements[firstIndex] = _elements[secondIndex];
        _elements[secondIndex] = temp;
    }

    public bool IsEmpty()
    {
        return _size == 0;
    }

    public Node Peek()
    {
        if (_size == 0)
            throw new IndexOutOfRangeException();

        return _elements[0];
    }

    public Node Pop()
    {
        if (_size == 0)
            throw new IndexOutOfRangeException();

        var result = _elements[0];
        _elements[0] = _elements[_size - 1];
        _size--;

        ReCalculateDown();

        return result;
    }

    public void Add(Node element)
    {
        if (_size == _elements.Length)
            throw new IndexOutOfRangeException();

        _elements[_size] = element;
        _size++;

        ReCalculateUp();
    }

    public void ReCalculateDown()
    {
        int index = 0;
        while (HasLeftChild(index))
        {
            var smallerIndex = GetLeftChildIndex(index);
            if (HasRightChild(index) && GetRightChild(index).dist < GetLeftChild(index).dist)
            {
                smallerIndex = GetRightChildIndex(index);
            }

            if (_elements[smallerIndex].dist >= _elements[index].dist)
            {
                break;
            }

            Swap(smallerIndex, index);
            index = smallerIndex;
        }
    }

    public void ReCalculateUp()
    {
        var index = _size - 1;
        while (!IsRoot(index) && _elements[index].dist < GetParent(index).dist)
        {
            var parentIndex = GetParentIndex(index);
            Swap(parentIndex, index);
            index = parentIndex;
        }
    }
}
