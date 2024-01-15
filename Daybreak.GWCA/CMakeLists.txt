# CMakeList.txt : CMake project for Daybreak.GWCA, include source and define
# project specific logic here.
#

project(Daybreak.GWCA)

if(CMAKE_SIZEOF_VOID_P EQUAL 8)
    message(FATAL_ERROR "You are configuring a 64bit build, this is not supported. Run cmake with `-A Win32`")
endif()

set_property(GLOBAL PROPERTY USE_FOLDERS ON)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(Daybreak.GWCA_FOLDER "${CMAKE_CURRENT_LIST_DIR}/")
list(APPEND CMAKE_MODULE_PATH "${CMAKE_CURRENT_LIST_DIR}/cmake/")

add_library(Daybreak.GWCA SHARED)
file(GLOB SOURCES
    "source/pch.h"
    "source/*.cpp"
    "source/*/*.cpp"
    "header/*.h*"
    "header/*/*.h")
source_group(TREE "${CMAKE_CURRENT_SOURCE_DIR}" FILES ${SOURCES})
target_sources(Daybreak.GWCA PRIVATE ${SOURCES})
target_compile_features(Daybreak.GWCA PUBLIC cxx_std_17)
target_compile_definitions(Daybreak.GWCA PRIVATE
    $<$<CONFIG:Debug>:BUILD_TYPE_DEBUG>
    $<$<CONFIG:Release>:BUILD_TYPE_RELEASE>
    NOMINMAX
)

target_precompile_headers(Daybreak.GWCA PRIVATE "header/pch.h")
target_include_directories(Daybreak.GWCA PUBLIC "header")

target_link_libraries(Daybreak.GWCA PUBLIC
    gwca
	directx
    minhook)