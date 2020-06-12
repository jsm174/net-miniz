cmake_minimum_required(VERSION 3.14)

include(FetchContent)

set(PROJECT_VERSION "2.1.0")

project(miniz VERSION ${PROJECT_VERSION})

FetchContent_Declare(minizsrc
   URL "https://github.com/richgel999/miniz/releases/download/${PROJECT_VERSION}/miniz-${PROJECT_VERSION}.zip"
)

FetchContent_Populate(minizsrc)

add_library(miniz SHARED
   ${minizsrc_SOURCE_DIR}/miniz.c
   ${CMAKE_CURRENT_SOURCE_DIR}/wrapper.cpp
)

target_include_directories(miniz PUBLIC
   ${minizsrc_SOURCE_DIR}
)

if(MSVC)
   set_target_properties(miniz 
      PROPERTIES VERSION ${PROJECT_VERSION}
      RUNTIME_OUTPUT_DIRECTORY_RELEASE "${CMAKE_BINARY_DIR}/lib"
      RUNTIME_OUTPUT_NAME "libminiz-${PROJECT_VERSION}"
   )
else()
   set_target_properties(miniz 
      PROPERTIES VERSION ${PROJECT_VERSION}
      LIBRARY_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/lib"
   )
endif()

file(COPY ${minizsrc_SOURCE_DIR}/LICENSE ${minizsrc_SOURCE_DIR}/ChangeLog.md
     DESTINATION ${CMAKE_BINARY_DIR}/lib)

