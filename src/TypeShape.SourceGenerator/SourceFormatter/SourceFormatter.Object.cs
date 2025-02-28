﻿using TypeShape.Roslyn;
using TypeShape.SourceGenerator.Model;

namespace TypeShape.SourceGenerator;

internal static partial class SourceFormatter
{
    private static void FormatObjectTypeShapeFactory(SourceWriter writer, string methodName, ObjectShapeModel objectShapeModel)
    {
        string? propertiesFactoryMethodName = objectShapeModel.Properties.Length > 0 ? $"CreateProperties_{objectShapeModel.Type.GeneratedPropertyName}" : null;
        string? constructorFactoryMethodName = objectShapeModel.Constructors.Length > 0 ? $"CreateConstructors_{objectShapeModel.Type.GeneratedPropertyName}" : null;

        writer.WriteLine($$"""
            private ITypeShape<{{objectShapeModel.Type.FullyQualifiedName}}> {{methodName}}()
            {
                return new SourceGenTypeShape<{{objectShapeModel.Type.FullyQualifiedName}}>
                {
                    Provider = this,
                    IsRecord = {{FormatBool(objectShapeModel.IsRecord)}},
                    CreatePropertiesFunc = {{FormatNull(propertiesFactoryMethodName)}},
                    CreateConstructorsFunc = {{FormatNull(constructorFactoryMethodName)}},
                };
            }
            """, trimNullAssignmentLines: true);

        if (propertiesFactoryMethodName != null)
        {
            writer.WriteLine();
            FormatPropertyFactory(writer, propertiesFactoryMethodName, objectShapeModel);
        }

        if (constructorFactoryMethodName != null)
        {
            writer.WriteLine();
            FormatConstructorFactory(writer, constructorFactoryMethodName, objectShapeModel);
        }
    }
}
