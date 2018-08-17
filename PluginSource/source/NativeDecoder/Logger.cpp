#include "Logger.h"
#include <memory>
#include <ctime>
#include <string>
#include <consoleapi.h>

#pragma warning(disable : 4996)
Logger *Logger::_instance;
UnityLog Logger::_unity = NULL;
Logger::~Logger()
{
	fclose(file);
	file = nullptr;
}
Logger::Logger()
{
	fclose(stdout);
	AllocConsole();
	freopen_s(&file, "CONOUT$", "wb", stdout);
	//file = freopen("NativeLog.txt", "a", stdout);
}

Logger *Logger::instance()
{
	if (!_instance)
	{
		_instance = new Logger();
	}
	return _instance;
}

void Logger::log(const char *str, ...)
{
	va_list args;
	va_start(args, str);
	char msg[500];
	size_t size = vsprintf(msg, str, args);
	if (_unity != NULL)
	{
		_unity(msg);
	}
	else
	{
		printf(msg);
	}
	va_end(args);

	fflush(stdout);
}

//void TimeProfiler::start(std::string tag)
//{
//	watches[tag] = clock();
//}
//
//void TimeProfiler::stop(std::string tag)
//{
//	auto start = watches.find(tag);
//	if (start != watches.end())
//	{
//		auto delta = (double)(clock() - start->second) / CLOCKS_PER_SEC * 1000;
//		LOG("[Time] [%s]: %f\n", tag.c_str(), delta);
//	}
//}
//
//TimeProfiler::TimeProfiler(const char *t) : tag(t)
//{
//	start(tag);
//}
//
//TimeProfiler::~TimeProfiler()
//{
//	stop(tag);
//}