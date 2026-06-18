# Passenger Picture Storage Migration Runbook

## Goal

Move picture storage from database (`PictureBinary`) to file system and enable Passenger upload compression to reduce future storage growth.

## Prerequisites

- Deployment package includes Passenger upload compression changes.
- Application pool/service account has read/write access to the image path (default `wwwroot/images`).
- Maintenance window scheduled (migration can be long on large datasets).

## 1) Backup (mandatory)

1. Take a full SQL backup.
2. Copy the current image root (if it already exists) to a safe location.
3. Record current media settings values for rollback:
   - `Media.Images.StoreInDB`
   - `MediaSettings.PicturePath`
   - `PassengerPictureSettings.TargetUploadSizeKb`

## 2) Configure compression target

1. Open admin panel: `Configuration -> Settings -> Media settings`.
2. Set `Passenger upload target size (KB)` to `300` (or the desired value).
3. Save settings.

## 3) Switch storage mode to file system

1. Open admin panel: `Configuration -> Settings -> Media settings`.
2. In `Pictures are stored into...`, click `Change` (to switch from database to file system).
3. Wait for completion. This triggers bulk migration through `SetIsStoreInDbAsync`.

## 4) Verify migration result

Run these SQL checks:

```sql
-- 4.1 Current storage flag (expect false)
SELECT [Value]
FROM [Setting]
WHERE [Name] = 'Media.Images.StoreInDB';

-- 4.2 Picture binary payload after migration (expect zero or very low)
SELECT COUNT(*) AS RowsWithBinaryData
FROM [PictureBinary]
WHERE [BinaryData] IS NOT NULL
  AND DATALENGTH([BinaryData]) > 0;

-- 4.3 Total rows still linked (metadata remains valid)
SELECT COUNT(*) AS TotalPictures
FROM [Picture];
```

Application checks:

- Upload a Passenger image larger than 300KB.
- Ensure upload succeeds and passenger record keeps a valid `PictureId`.
- Confirm resulting stored image is reduced near the configured target.
- Confirm thumbnail generation still works under `wwwroot/images/thumbs`.
- Confirm new `PictureBinary` rows have `BinaryData IS NULL` (or `DATALENGTH=0`), not non-empty payload.

## 5) Rollback plan

If there is an issue:

1. Switch `Pictures are stored into...` back to `database`.
2. Restore SQL backup if data integrity is in doubt.
3. Restore the image folder backup if needed.
4. Temporarily disable Passenger-specific compression by setting `Passenger upload target size (KB)` to a high value (for example `20480`).

## Notes

- Thumbnail folder growth (`wwwroot/images/thumbs`) is expected behavior.
- Primary DB growth pressure is reduced when `Media.Images.StoreInDB = false`.
