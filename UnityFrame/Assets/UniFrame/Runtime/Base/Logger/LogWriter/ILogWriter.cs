
internal interface ILogWriter
{
	void AddError(string message);

	void AddInfo(string message, string gName = null);

	void Start();

	void Stop();

	void Flush();

	void Pause();

	void Resume();
}
