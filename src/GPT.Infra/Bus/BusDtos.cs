#nullable disable

using System.Text.Json;
using System.Text.Json.Serialization;

namespace GPT.Bus;

internal class LineDto
{
    [JsonPropertyName("LineCode")]
    public string LineCode { get; init; }

    [JsonPropertyName("LineID")]
    public string LineId { get; init; }

    [JsonPropertyName("LineDescr")]
    public string DescriptionGr { get; init; }

    [JsonPropertyName("LineDescrEng")]
    public string DescriptionEn { get; init; }
}

internal class RouteDto
{
    [JsonPropertyName("route_code")]
    public string RouteCode { get; set; }

    [JsonPropertyName("route_descr")]
    public string DescriptionEn { get; set; }

    [JsonPropertyName("route_descr_eng")]
    public string DescriptionGr { get; set; }

    public class Stop
    {
        [JsonPropertyName("StopCode")]
        public string StopCode { get; init; }

        [JsonPropertyName("StopDescr")]
        public string DescriptionGr { get; init; }

        [JsonPropertyName("StopDescrEng")]
        public string DescriptionEn { get; init; }

        [JsonPropertyName("StopLat")]
        public string Latitude { get; init; }

        [JsonPropertyName("StopLng")]
        public string Longitude { get; init; }

        [JsonPropertyName("StopAmea")]
        public string IsAmea { get; init; }
    }
}

internal class StopDto
{
    [JsonPropertyName("stop_id")]
    public string StopCode { get; init; }

    [JsonPropertyName("stop_descr")]
    public string DescriptionGr { get; init; }

    [JsonPropertyName("stop_descr_matrix_eng")]
    public string DescriptionEn { get; init; }

    [JsonPropertyName("stop_lat")]
    public string Latitude { get; init; }

    [JsonPropertyName("stop_lng")]
    public string Longitude { get; init; }

    public class Route
    {
        [JsonPropertyName("RouteCode")]
        public string RouteCode { get; init; }

        [JsonPropertyName("LineCode")]
        public string LineCode { get; init; }

        [JsonPropertyName("LineID")]
        public string LineId { get; init; }

        [JsonPropertyName("hidden")]
        public string Hidden { get; init; }

        [JsonPropertyName("RouteDescr")]
        public string DescriptionGr { get; init; }

        [JsonPropertyName("RouteDescrEng")]
        public string DescriptionEn { get; init; }
    }
}

internal class ScheduleDto
{
    [JsonPropertyName("sdc_descr")]
    public string SdcDescrGr { get; init; }

    [JsonPropertyName("sdc_descr_eng")]
    public string SdcDescrEng { get; init; }

    [JsonPropertyName("sdc_code")]
    public string SdcCode { get; init; }
}

[JsonConverter(typeof(ScheduleDtoConverter))]
internal class ScheduleTimetableDto
{
    public TimeOnly[] Go { get; init; }

    public TimeOnly[] Come { get; init; }

    private class ScheduleDtoConverter : JsonConverter<ScheduleTimetableDto>
    {
        public override ScheduleTimetableDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            TimeOnly[] come = Array.Empty<TimeOnly>();
            TimeOnly[] go = Array.Empty<TimeOnly>();

            while (reader.Read())
            {
                if (reader.TokenType is JsonTokenType.EndObject && reader.CurrentDepth is 0) break;
                if (reader.TokenType is JsonTokenType.PropertyName)
                {
                    var name = reader.GetString();
                    if (name is "come")
                    {
                        come = ReadArray(ref reader, "sde_start2");
                    }
                    else if (name is "go")
                    {
                        go = ReadArray(ref reader, "sde_start1");
                    }
                }
            }

            return new ScheduleTimetableDto { Come = come, Go = go };
        }

        private static TimeOnly[] ReadArray(ref Utf8JsonReader reader, string propertyName)
        {
            reader.Read();
            reader.Read();
            if (reader.TokenType is JsonTokenType.EndArray)
            {
                return Array.Empty<TimeOnly>();
            }

            var list = new List<TimeOnly>();
            while (reader.Read())
            {
                if (reader.TokenType is JsonTokenType.EndArray) break;
                if (reader.TokenType is JsonTokenType.PropertyName &&
                    reader.GetString() == propertyName)
                {
                    reader.Read();
                    var str = reader.GetString();
                    if (str is { })
                    {
                        list.Add(TimeOnly.Parse(str[11..]));
                    }
                }
            }
            return list.ToArray();
        }

        public override void Write(Utf8JsonWriter writer, ScheduleTimetableDto value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}

internal class ArrivalDto
{
    [JsonPropertyName("route_code")]
    public string RouteCode { get; init; }

    [JsonPropertyName("veh_code")]
    public string VehicleCode { get; init; }

    [JsonPropertyName("btime2")]
    public int Time { get; set; }
}

#nullable restore
