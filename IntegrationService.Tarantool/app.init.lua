#!/usr/bin/env tarantool

local log = require('log')
local uuid = require('uuid')

local function init()
    box.schema.user.grant('guest', 'read,write,execute,create,drop', 'universe', nil, {
        if_not_exists = true
    })

    local author_space = box.schema.space.create('author', {
        if_not_exists = true
    })

    local bookauthor_space = box.schema.space.create('bookauthor', {
        if_not_exists = true
    })

    local book_space = box.schema.space.create('book', {
        if_not_exists = true
    })

    author_space:create_index('primary_id', {
        if_not_exists = true,
        type = 'HASH',
        unique = true,
        parts = {1, 'INT'}
    })

    bookauthor_space:create_index('primary_id', {
        if_not_exists = true,
        type = 'HASH',
        unique = true,
        parts = {1, 'INT'}
    })

    book_space:create_index('primary_id', {
        sif_not_exists = true,
        type = 'HASH',
        unique = true,
        parts = {1, 'INT'}
    })
end

box.cfg
{
    pid_file = nil,
    background = false,
    log_level = 5
}

box.once('init', init)