# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

---

## [1.0.0] — 2026

### Added
- Detect presence of `C:\Windows\System32\drivers\etc\hosts` at startup
- Display hosts file content in a read-only Consolas text box
- Timestamped backup to local `Backups/` folder (`hosts_YYYYMMDD_HHmmss.bak`)
- Dropdown list of available backups, sorted newest-first
- Restore to Windows factory default content
- Restore from any selected `.bak` backup
- Read-Only attribute strip-before / re-lock-after pipeline for every write
- Silent `ipconfig /flushdns` after every restore
- Guard against restoring empty or whitespace-only backup files
- BOM-free UTF-8 encoding on all reads and writes
- UAC elevation at launch (auto-restart with `runas` verb)
- Windows version gate: requires build 7601 (Windows 7 SP1) or later
- Application icon (48 × 32 × 16 px shield design, PNG-in-ICO)
