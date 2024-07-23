using System;
using System.Activities.Presentation.Metadata;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace FWBS.OMS.Workflow.Admin
{
    internal class LoadVSIcons
	{
		#region Load icons from VS assembly
		//
		// The icons for the builtin activities are in a Microsoft DLL which is NOT a redistributable at this moment in time
		//	Thus we use our own bitmaps. When Microsoft releases a DLL with the resources in it then load that assembly up... 
		//
		internal static void LoadToolboxIconsForBuiltInActivities()
		{
			AttributeTableBuilder builder = new AttributeTableBuilder();

			Assembly sourceAssembly = Assembly.GetAssembly(typeof(LoadVSIcons));
			System.Resources.ResourceReader resourceReader = new System.Resources.ResourceReader(sourceAssembly.GetManifestResourceStream("FWBS.OMS.Workflow.Resources.BuiltInResources.resources"));

			// Extract standard types
			foreach (Type type in typeof(System.Activities.Activity).Assembly.GetTypes().Where(t => t.Namespace == "System.Activities.Statements"))
			{
				CreateToolboxBitmapAttributeForActivity(builder, resourceReader, type);
			}

			// Messaging types
			foreach (Type type in typeof(System.ServiceModel.Activities.InitializeCorrelation).Assembly.GetTypes().Where(t => t.Namespace == "System.ServiceModel.Activities"))
			{
				CreateToolboxBitmapAttributeForActivity(builder, resourceReader, type);
			}

			// Messaging types - factories
			foreach (Type type in typeof(System.ServiceModel.Activities.Presentation.Factories.SendAndReceiveReplyFactory).Assembly.GetTypes().Where(t => t.Namespace == "System.ServiceModel.Activities.Presentation.Factories"))
			{
				System.Drawing.Bitmap bitmap = ExtractBitmapResource(resourceReader, type.IsGenericType ? type.Name.Split('`')[0] : type.Name.Replace("Factory", string.Empty));
				if (bitmap != null)
				{
					Type tbaType = typeof(System.Drawing.ToolboxBitmapAttribute);
					Type imageType = typeof(System.Drawing.Image);
					ConstructorInfo constructor = tbaType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { imageType, imageType }, null);
					System.Drawing.ToolboxBitmapAttribute tba = constructor.Invoke(new object[] { bitmap, bitmap }) as System.Drawing.ToolboxBitmapAttribute;
					builder.AddCustomAttributes(type, tba);
				}
			}

			// Validation types
			foreach (Type type in typeof(System.Activities.Validation.AssertValidation).Assembly.GetTypes().Where(t => t.Namespace == "System.Activities.Validation"))
			{
				CreateToolboxBitmapAttributeForActivity(builder, resourceReader, type);
			}

			MetadataStore.AddAttributeTable(builder.CreateTable());
		}

		private static void CreateToolboxBitmapAttributeForActivity(AttributeTableBuilder builder, System.Resources.ResourceReader resourceReader, Type builtInActivityType)
		{
			System.Drawing.Bitmap bitmap = ExtractBitmapResource(resourceReader, builtInActivityType.IsGenericType ? builtInActivityType.Name.Split('`')[0] : builtInActivityType.Name);
			if (bitmap != null)
			{
				Type tbaType = typeof(System.Drawing.ToolboxBitmapAttribute);
				Type imageType = typeof(System.Drawing.Image);
				ConstructorInfo constructor = tbaType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { imageType, imageType }, null);
				System.Drawing.ToolboxBitmapAttribute tba = constructor.Invoke(new object[] { bitmap, bitmap }) as System.Drawing.ToolboxBitmapAttribute;
				builder.AddCustomAttributes(builtInActivityType, tba);
			}
		}

		private static System.Drawing.Bitmap ExtractBitmapResource(System.Resources.ResourceReader resourceReader, string bitmapName)
		{
			System.Collections.IDictionaryEnumerator dictEnum = resourceReader.GetEnumerator();
			System.Drawing.Bitmap bitmap = null;
			while (dictEnum.MoveNext())
			{
				if (String.Equals(dictEnum.Key, bitmapName))
				{
					bitmap = dictEnum.Value as System.Drawing.Bitmap;
					System.Drawing.Color pixel = bitmap.GetPixel(bitmap.Width - 1, 0);
					bitmap.MakeTransparent(pixel);
					break;
				}
			}

			return bitmap;
		}
		#endregion

        static ResourceDictionary iconsDict;

        /// <summary>
        /// Loads the standard activity icons from System.Activities.Presentation.
        /// Accepts various forms of input including 'short name' i.e. toolbox display name, and 'Type.FullName' - and tries to fix them automatically.
        /// </summary>
        /// <param name="iconOrtypeName"></param>
        /// <returns></returns>
        public static object ExtractIconResource(string iconOrtypeName)
        {
            iconsDict = iconsDict ?? new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/System.Activities.Presentation;component/themes/icons.xaml")
            };

            string resourceKey = GetResourceName(iconOrtypeName);
            object resource = iconsDict.Contains(resourceKey) ? iconsDict[resourceKey] : null;

            if (!(resource is DrawingBrush))
            {
                resource = iconsDict["GenericLeafActivityIcon"];
            }

            return resource;
        }

        /// <summary>
        /// Extract Icon Resource
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ExtractIconResource(Type type)
        {
            string typeName = type.IsGenericType ? type.GetGenericTypeDefinition().Name : type.Name;
            return ExtractIconResource(typeName);
        }

        /// <summary>
        /// Get Resource Name
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        static string GetResourceName(string typeName)
        {
            string resourceKey = typeName;
            resourceKey = resourceKey.Split('.').Last();
            resourceKey = resourceKey.Split('`').First();

            if (resourceKey == "Flowchart")
            {
                // it appears that themes/icons.xaml has a typo here
                resourceKey = "FlowChart";
            }

            if (!resourceKey.EndsWith("Icon"))
            {
                resourceKey += "Icon";
            }

            return resourceKey;
        }
	}
}
