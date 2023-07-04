# LineObjImporter

拡張子 `.lineobj` の Importer です. デフォルトでは Face の存在しない `.fbx` を読み込むことができないため, Edge のみが含まれる Geometry を Asset
として取り扱うために使用します.

## 作成方法 (Houdini)

- [ROP Geometry ROP](https://www.sidefx.com/docs/houdini/nodes/out/geometry.html) を使用して `.obj` 形式で書き出し
- 拡張子を `.obj` -> `.lineobj` にリネーム
- Unity 内にインポート