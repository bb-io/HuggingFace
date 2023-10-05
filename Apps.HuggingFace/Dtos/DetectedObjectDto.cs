namespace Apps.HuggingFace.Dtos;

public record DetectedObjectDto(string Label, double Score, BoxDto Box);

public record BoxDto(int Xmin, int Ymin, int Xmax, int Ymax);