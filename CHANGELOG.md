# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

---

## [Unreleased]

### Security
- **Backups folder hardened**: moved from a folder next to the `.exe` (often Desktop/Downloads,
  writable by any non-elevated process run by the same user) to
  `%ProgramData%\HostsFileRecoveryTool\Backups\`, created with an ACL restricted to
  Administrators/SYSTEM. Prevents a non-elevated process from planting a malicious `.bak` file
  that later gets written into the system `hosts` file via "Restore from Backup" run elevated.

### Added
- Automatic snapshot of the current `hosts` file right before any restore overwrites it
  (`autosave_before_restore_*.bak`), so an accidental "Restore Default" click is always recoverable.
- Atomic write for restores (write to a temp file, then swap into place) so a crash or power loss
  mid-write can no longer leave the `hosts` file truncated/corrupted.

### Fixed
- Startup OS-detection/backup-listing logic unaffected; backup filenames now include milliseconds
  (`hosts_YYYYMMDD_HHmmss_fff.bak`) to avoid same-second collisions from rapid clicks.

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
