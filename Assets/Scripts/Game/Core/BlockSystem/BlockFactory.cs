using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

public static class BlockFactory
{
    private static Dictionary<BlockTypes, Type> blocksByName;
    private static bool IsInitialized => blocksByName != null;
    private static void InitBlockFactory()
    {
        if (IsInitialized)
        {
            return;
        }
        var blockTypes = Assembly.GetAssembly(typeof(Block)).GetTypes()
           .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Block))
           && !myType.IsSubclassOf(typeof(CubeBlock)));

        blocksByName = new Dictionary<BlockTypes, Type>();

        foreach (var type in blockTypes)
        {
            var temp = Activator.CreateInstance(type) as Block;
            blocksByName.Add(temp.blockType, type);
        }
    }
    public static void Reset()
    {
        var blockTypes = Assembly.GetAssembly(typeof(Block)).GetTypes()
           .Where(myType => myType.IsClass && myType.IsSubclassOf(typeof(Block))
           && !myType.IsSubclassOf(typeof(CubeBlock)));

        blocksByName = new Dictionary<BlockTypes, Type>();

        foreach (var type in blockTypes)
        {
            var temp = Activator.CreateInstance(type) as Block;
            blocksByName.Add(temp.blockType, type);
        }
    }
    public static Block GetBlock(BlockTypes blockType)
    {
        InitBlockFactory();
        if (blocksByName.ContainsKey(blockType))
        {
            Type type = blocksByName[blockType];
            var block = Activator.CreateInstance(type) as Block;
            return block;
        }
        return null;
    }

}
