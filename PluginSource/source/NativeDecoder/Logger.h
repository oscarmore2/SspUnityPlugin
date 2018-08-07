//========= Copyright 2015-2018, HTC Corporation. All rights reserved. ===========

#pragma once
#include <stdio.h>
#include <stdarg.h>
#include <string>
#include <map>

#define ENABLE_LOG
#ifdef ENABLE_LOG
#define LOG(...) Logger::instance()->log(__VA_ARGS__)
#else
#define LOG
#endif
typedef void(__stdcall *UnityLog)(const char *str);
class Logger
{
  public:
	static Logger *instance();
	void log(const char *str, ...);
	static UnityLog _unity;

  protected:
	virtual ~Logger();

  private:
	Logger();
	static Logger *_instance;
	FILE *file;
};

#define ENABLE_PROFILER
#ifdef ENABLE_PROFILER
#define PROFILE_TIME_START(tag) TimeProfiler::start(#tag);
#define PROFILE_TIME_END(tag) TimeProfiler::end(#tag);
#define PROFILE_TIME_ONCE(tag) TimeProfiler __profile_time##__LINE__(#tag);
#else
#define PROFILE_TIME_START(tag) 
#define PROFILE_TIME_END(tag) 
#define PROFILE_TIME_ONCE(tag) 
#endif

class TimeProfiler
{
  public:
	TimeProfiler(const char *t);
	~TimeProfiler();

	static void start(std::string tag);
	static void stop(std::string tag);

  private:
	std::string tag;
	static std::map<std::string, clock_t> watches;
};