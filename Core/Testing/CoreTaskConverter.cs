using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using STLib.Tasks.Checkboxes;
using STLib.Tasks.MultipleChoice;
using STLib.Tasks.Text;
using STLib.Tasks.TrueFalse;

namespace STLib.Core.Testing
{
    /// <summary>
    /// Custom JSON converter for handling polymorphic serialization and deserialization of <see cref="CoreTask"/> objects.
    /// </summary>
    public sealed class CoreTaskConverter : JsonConverter<CoreTask>
    {
        /// <summary>
        /// Reads and deserializes a <see cref="CoreTask"/> object from JSON, determining its specific type based on the "type" property.
        /// </summary>
        /// <param name="reader">The <see cref="Utf8JsonReader"/> to read JSON data.</param>
        /// <param name="typeToConvert">The target type to convert.</param>
        /// <param name="options">The serialization options to use.</param>
        /// <returns>A deserialized <see cref="CoreTask"/> object.</returns>
        /// <exception cref="NotSupportedException">Thrown when the task type is not supported.</exception>
        /// <exception cref="JsonException">Thrown when the "type" discriminator is missing in the JSON.</exception>
        public override CoreTask Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            if (root.TryGetProperty("type".AsSpan(), out var typeProperty))
            {
                if (Enum.TryParse<TaskType>(typeProperty.GetString(), true, out var type))
                {
                    CoreTask? task = type switch
                    {
                        TaskType.Text => JsonSerializer.Deserialize<TextTask>(root.GetRawText(), options),
                        TaskType.MultipleChoice => JsonSerializer.Deserialize<MultipleChoiceTask>(root.GetRawText(), options),
                        TaskType.Checkboxes => JsonSerializer.Deserialize<CheckboxesTask>(root.GetRawText(), options),
                        TaskType.TrueFalse => JsonSerializer.Deserialize<TrueFalseTask>(root.GetRawText(), options),
                        TaskType.Unknown => throw new NotSupportedException($"Type \"{type}\" not supported."),
                        _ => throw new NotSupportedException($"Type \"{type}\" not supported.")
                    };

                    if (task == null)
                    {
                        return TextTask.Build();
                    }

                    return task;
                }

                throw new NotSupportedException($"Type \"{type}\" not supported.");
            }

            throw new JsonException("Missing Type discriminator.");
        }
        /// <summary>
        /// Serializes a <see cref="CoreTask"/> object to JSON, including its specific type information.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write JSON data.</param>
        /// <param name="value">The <see cref="CoreTask"/> object to serialize.</param>
        /// <param name="options">The serialization options to use.</param>
        public override void Write(Utf8JsonWriter writer, CoreTask value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}