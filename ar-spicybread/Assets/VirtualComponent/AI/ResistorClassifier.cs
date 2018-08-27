//#if WINDOWS_UWP
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Windows.Media;
//using Windows.Storage;
//using Windows.AI.MachineLearning.Preview;
//using Windows.Foundation;

//// 8eb8c71d-3088-4f73-a579-cf74047d8d0b_0d0027f5-95d9-414f-b16b-159f83a2aa86

//namespace Breadboard
//{
//    namespace AI
//    {
//        namespace ResistorClassifier
//        {
//            public sealed class ResistorClassifierModelInput : IModelInput
//            {
//                public VideoFrame data { get; set; }
//            }

//            public sealed class ResistorClassifierModelOutput : IModelOutput
//            {
//                public IList<string> classLabel { get; set; }
//                public IDictionary<string, float> loss { get; set; }
//                public ResistorClassifierModelOutput()
//                {
//                    this.classLabel = new List<string>();
//                    this.loss = new Dictionary<string, float>()
//                    {
//                        { "bot", float.NaN },
//                        { "empty", float.NaN },
//                        { "leg", float.NaN },
//                        { "resistor", float.NaN },
//                        { "top", float.NaN },
//                    };
//                }
//            }

//            public sealed class ResistorClassifierModel : IModel
//            {
//                private LearningModelPreview learningModel;
//                public static IAsyncOperation<ResistorClassifierModel> CreateResistorClassifierModel(StorageFile file)
//                {
//                    return Task.Run(async () =>
//                    {
//                        LearningModelPreview learningModel = await LearningModelPreview.LoadModelFromStorageFileAsync(file);
//                        ResistorClassifierModel model = new ResistorClassifierModel();
//                        model.learningModel = learningModel;
//                        return model;
//                    }).AsAsyncOperation();
//                }

//                public IModelInput CreateModelInput(VideoFrame data)
//                {
//                    var modelInput = new ResistorClassifierModelInput();
//                    modelInput.data = data;
//                    return modelInput;
//                }

//                public IAsyncOperation<IModelOutput> EvaluateAsync(IModelInput input)
//                {
//                    return Task.Run(async () =>
//                    {
//                        IModelOutput output = new ResistorClassifierModelOutput();
//                        LearningModelBindingPreview binding = new LearningModelBindingPreview(learningModel);
//                        binding.Bind("data", input.data);
//                        binding.Bind("classLabel", output.classLabel);
//                        binding.Bind("loss", output.loss);
//                        LearningModelEvaluationResultPreview evalResult = await learningModel.EvaluateAsync(binding, string.Empty);
//                        return output;
//                    }).AsAsyncOperation();
//                }
//            }
//        }
//    }
//}
//#endif
