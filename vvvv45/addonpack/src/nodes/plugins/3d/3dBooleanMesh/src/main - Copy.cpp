#include <igl/read_triangle_mesh.h>
#include <igl/copyleft/cgal/CSGTree.h>
#include <Eigen/Core>

#include "tutorial_shared_path.h"

int main(int argc, char * argv[])
{
	using namespace Eigen;
	using namespace igl::copyleft::cgal;
	using namespace std;
	using namespace igl;
	cout << R"(
[,]  Toggle between boolean sub-tree operations
)";

	MatrixXi FA, FB, FC, FD, FE;
	MatrixXd VA, VB, VC, VD, VE;
	// Read in inputs as double precision floating point meshes
	read_triangle_mesh(TUTORIAL_SHARED_PATH "/cube.obj", VA, FA);
	read_triangle_mesh(TUTORIAL_SHARED_PATH "/sphere.obj", VB, FB);
	read_triangle_mesh(TUTORIAL_SHARED_PATH "/xcylinder.obj", VC, FC);
	read_triangle_mesh(TUTORIAL_SHARED_PATH "/ycylinder.obj", VD, FD);
	read_triangle_mesh(TUTORIAL_SHARED_PATH "/zcylinder.obj", VE, FE);


	int num_views = 5 + 4;
	int view_id = num_views - 1;
	const auto & update = [&]()
	{

		// CSGTree templated on type of F
		VectorXd I;
		const auto & set_mesh =
			[&](const MatrixXd & V, const MatrixXi & F, const int i)
		{
			I = VectorXd::Constant(F.rows(), 1, i);
		};
		switch (view_id)
		{
		case 0:
			set_mesh(VA, FA, 5);
			break;
		case 1:
			set_mesh(VB, FB, 4);
			break;
		case 2:
			set_mesh(VC, FC, 3);
			break;
		case 3:
			set_mesh(VD, FD, 2);
			break;
		case 4:
			set_mesh(VE, FE, 1);
			break;
		default:
		{
			CSGTree M;
			Matrix<MatrixXi::Index, Dynamic, 1> J;
			switch (view_id)
			{
			case 5:
				// Compute result of (A ∩ B)
				M = { {VA,FA},{VB,FB},"i" };
				J = M.J().array() + 0;
				break;
			case 6:
				// Compute result of (C ∪ D)
				M = { {VC,FC},{VD,FD},"u" };
				J = M.J().array() + FA.rows() + FB.rows();
				break;
			case 7:
				// Compute result of (C ∪ D) ∪ E
				M = { {{VC,FC},{VD,FD},"u"},{VE,FE},"u" };
				J = M.J().array() + FA.rows() + FB.rows();
				break;
			case 8:
				// Compute result of (A ∩ B) \ ((C ∪ D) ∪ E)
				M = { {{VA,FA},{VB,FB},"i"},{{{VC,FC},{VD,FD},"u"},{VE,FE},"u"},"m" };
				J = M.J().array() + 0;
				break;
			default:
				assert(false && "unknown view id");
			}

			I.resize(M.F().rows(), 1);

		}
		}

		MatrixXd C;

	};
	update();

}
